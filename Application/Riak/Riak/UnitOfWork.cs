using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.ObjectBuilder;
using Demo.Application.Riak.Infrastructure;
using Newtonsoft.Json;
using RiakClient;
using RiakClient.Models;
using System.Collections.Concurrent;
using System.Configuration;
using Demo.Application.Riak.Infrastructure.Riak;
using Demo.Library.Queries;
using Demo.Application.Riak.Infrastructure.Extensions;
using System.Runtime.Serialization;
using NServiceBus.MessageInterfaces;
using Metrics;
using Demo.Library.Extensions;
using Demo.Application.Riak.Infrastructure.Exceptions;
using NServiceBus.UnitOfWork;
using NLog;

namespace Demo.Application.Riak
{
    public class UnitOfWork : IUnitOfWork
    {
        private static NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
        public IBuilder Builder { get; set; }

        private static Meter _readsMeter = Metric.Meter("Riak Reads", Unit.Items);
        private static Meter _writesMeter = Metric.Meter("Riak Writes", Unit.Items);
        private static Meter _deletesMeter = Metric.Meter("Riak Deletes", Unit.Items);

        private readonly IRiakClient _client;
        private readonly IMessageMapper _mapper;
        private ConcurrentDictionary<RiakObjectId, RiakObject> _tracked;
        private ConcurrentDictionary<RiakObjectId, RiakObject> _saves;
        private ConcurrentDictionary<RiakObjectId, RiakObject> _updates;
        private ConcurrentBag<RiakObjectId> _deletions;

        public UnitOfWork(IRiakClient client, IMessageMapper mapper)
        {
            _client = client;
            _mapper = mapper;
            _tracked = new ConcurrentDictionary<RiakObjectId, RiakObject>();
            _saves = new ConcurrentDictionary<RiakObjectId, RiakObject>();
            _updates = new ConcurrentDictionary<RiakObjectId, RiakObject>();
            _deletions = new ConcurrentBag<RiakObjectId>();
        }


        public async Task<IEnumerable<string>> Query<TPaged>(TPaged query) where TPaged : IPaged
        {
            _readsMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(TPaged), query.ToId()));

            if (_tracked.ContainsKey(id))
                return _tracked[id].GetObject<IEnumerable<string>>() ?? new string[] { };
            
            var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
            options.SetRw(Quorum.WellKnown.Quorum);

            var result = await _client.Async.Get(id, options);
            if (!result.IsSuccess)
                return new string[] { };

            _tracked[id] = result.Value;

            return result.Value.GetObject<IEnumerable<string>>() ?? new string[] { };
        }
        public Task<IEnumerable<string>> Query<TPaged>(Action<TPaged> query) where TPaged : IPaged
        {
            var result = _mapper.CreateInstance(query);
            return Query(result);
        }
        public void SaveQuery<TPaged>(Action<TPaged> query, IEnumerable<string> results) where TPaged : IPaged
        {
            var result = _mapper.CreateInstance(query);
            SaveQuery(result, results);
        }
        public void SaveQuery<TPaged>(TPaged query, IEnumerable<string> results) where TPaged : IPaged
        {
            _writesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(TPaged), query.ToId()));

            if (_tracked.ContainsKey(id))
            {
                var obj = _tracked[id];
                obj.SetObject(results);
                _updates[id] = obj;
            }
            else
            {
                var obj = new RiakObject(id, results);
                _saves[id] = obj;
                _tracked[id] = obj;
            }
        }

        public void DeleteQuery<TPaged>(TPaged query) where TPaged : IPaged
        {
            _deletesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(TPaged), query.ToId()));
            _deletions.Add(id);
        }
        public void DeleteQuery<TPaged>(Action<TPaged> query) where TPaged : IPaged
        {
            var result = _mapper.CreateInstance(query);
            DeleteQuery(result);
        }


        public async Task<IEnumerable<T>> Get<T>(IEnumerable<string> keys) where T : class
        {
            _readsMeter.Mark();
            var ids = keys.Select(key => new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(T), key)));

            var existing = _tracked.Keys.Intersect(ids);
            var except = ids.Except(existing);

            var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
            options.SetRw(Quorum.WellKnown.Quorum);

            if (except.Count() == 0)
                return existing.Select(x => _tracked[x].GetObject<T>());

            var results = await _client.Async.Get(except, options);

            foreach (var result in results.Where(x => x.IsSuccess))
                _tracked[result.Value.ToRiakObjectId()] = result.Value;
            
            var documents = new List<T>(existing.Select(x => _tracked[x].GetObject<T>()));
            return documents.Concat(results.Select(x => x.Value?.GetObject<T>()));
        }
        public Task<IEnumerable<T>> Get<T>(IEnumerable<ValueType> keys) where T: class
        {
            return Get<T>(keys.Select(x => x.ToString()));
        }


        public async Task<T> Get<T>(string key) where T : class
        {
            _readsMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(T), key));

            if (_tracked.ContainsKey(id))
                return _tracked[id].GetObject<T>();

            var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
            options.SetRw(Quorum.WellKnown.Quorum);

            var result = await _client.Async.Get(id, options);
            if (!result.IsSuccess)
                return default(T);

            _tracked[id] = result.Value;

            return result.Value.GetObject<T>();
        }
        public async Task<T> Get<T>(ValueType id) where T : class
        {
            return await Get<T>(id.ToString());
        }
        public void Delete<T>(string key) where T : class
        {
            _deletesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(T), key));
            _deletions.Add(id);
        }
        public void Delete<T>(ValueType id) where T : class
        {
            Delete<T>(id.ToString());
        }
        public void Delete<T>(T doc) where T : class
        {
            var idField = typeof(T).GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (idField == null)
                throw new ArgumentException($"No ID field in document {typeof(T).FullName}");

            var documentKey = idField.GetValue(doc).ToString();
            Delete<T>(documentKey);
        }
        public void Save<T>(string key, T doc) where T : class
        {
            _writesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(T), key));

            if (_tracked.ContainsKey(id))
            {
                var obj = _tracked[id];
                obj.SetObject(doc);
                _updates[id] = obj;
            }
            else
            {
                var obj = new RiakObject(id, doc);
                _saves[id] = obj;
                _tracked[id] = obj;
            }
        }
        public void Save<T>(T doc) where T : class
        {
            var idField = typeof(T).GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (idField == null)
                throw new ArgumentException($"No ID field in document {typeof(T).FullName}");
            var key = idField.GetValue(doc).ToString();

            Save(key, doc);
        }
        public void Save<T>(ValueType id, T doc) where T : class
        {
            Save(id.ToString(), doc);
        }

        public Task Begin()
        {
            return Task.FromResult(true);
        }
        private async Task BackOutSaves(IEnumerable<RiakObjectId> keys)
        {
            // Backing out any saves that might of succeeded while we were saving, 
            var options = new RiakDeleteOptions { };
            options.SetR(Quorum.WellKnown.All);

            await _client.Async.Delete(keys);
        }


        public async Task End(Exception ex = null)
        {
            if (ex != null) return;

            // New and update options commented out for now.. causing too many events to be dropped because riak doing weird things
            var options = new RiakPutOptions { /*IfNoneMatch = true, IfNotModified = false, */ReturnHead = true, ReturnBody = false };
            options.SetW(Quorum.WellKnown.Quorum);
            var saved = _client.Async.Put(_saves.Values, options);

            var uptOptions = new RiakPutOptions {/* IfNoneMatch = false, IfNotModified = true,*/ ReturnHead = true, ReturnBody = false };
            uptOptions.SetW(Quorum.WellKnown.Quorum);
            var updated = _client.Async.Put(_updates.Values, uptOptions);

            var deleteOpt = new RiakDeleteOptions();
            deleteOpt.SetR(Quorum.WellKnown.Quorum);
            var deleted = _client.Async.Delete(_deletions, deleteOpt);

            await Task.WhenAll(saved, updated, deleted);

            var exceptions = saved.Result.Where(x => !x.IsSuccess).Select(x =>  x.Exception)
                .Concat(updated.Result.Where(x => !x.IsSuccess).Select(x => x.Exception))
                .Concat(deleted.Result.Where(x => !x.IsSuccess).Select(x => x.Exception));

            if (exceptions.Any())
            {
                // Backout saves because if the event created new objects running the event again will cause more errors due to match_found
                await BackOutSaves(_saves.Keys);
                throw new StorageException("Failed to commit", exceptions); 
            }

        }

    }
}
