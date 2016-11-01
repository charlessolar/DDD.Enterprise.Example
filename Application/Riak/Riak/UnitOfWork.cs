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
using Aggregates.Contracts;
using NServiceBus.MessageInterfaces;
using Metrics;
using Demo.Library.Extensions;
using Demo.Application.Riak.Infrastructure.Exceptions;
using Demo.Library.Caching;
using NServiceBus.UnitOfWork;
using NLog;
using NServiceBus;

namespace Demo.Application.Riak
{
    public class UnitOfWork : IUnitOfWork, IEventMutator
    {
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
        public IBuilder Builder { get; set; }
        public int Retries { get; set; }

        private static readonly IntelligentCache MemCache = new IntelligentCache();
        private static readonly Meter ReadsMeter = Metric.Meter("Riak Reads", Unit.Items);
        private static readonly Meter WritesMeter = Metric.Meter("Riak Writes", Unit.Items);
        private static readonly Meter DeletesMeter = Metric.Meter("Riak Deletes", Unit.Items);
        private static readonly Meter ExceptionsMeter = Metric.Meter("Riak Exceptions", Unit.Items);

        private static readonly Metrics.Timer CommitTimer = Metric.Timer("Riak Commit Time", Unit.Items);

        private static readonly Meter CacheHits = Metric.Meter("Riak Cache Hits", Unit.Items);
        private static readonly Meter CacheMisses = Metric.Meter("Riak Cache Misses", Unit.Items);

        private readonly IRiakClient _client;
        private readonly IMessageMapper _mapper;
        private readonly ConcurrentDictionary<RiakObjectId, RiakObject> _tracked;
        private readonly ConcurrentDictionary<RiakObjectId, RiakObject> _saves;
        private readonly ConcurrentDictionary<RiakObjectId, RiakObject> _updates;
        private readonly ConcurrentBag<RiakObjectId> _deletions;

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
            ReadsMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(TPaged), query.ToId()));

            if (_tracked.ContainsKey(id))
                return _tracked[id].GetObject<IEnumerable<string>>() ?? new string[] { };


            Logger.Debug("Getting query id {0}", id.Key);

            var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
            options.SetRw(Quorum.WellKnown.Quorum);
            var result = await _client.Async.Get(id, options).ConfigureAwait(false);
            if (!result.IsSuccess)
            {
                Logger.Debug("Failed to get key {0} for query {1} from riak - Error: {2} {3}", id.Key, typeof(TPaged).FullName, result.ResultCode, result.ErrorMessage);
                return new string[] { };
            }
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
            WritesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(TPaged), query.ToId()));

            if (_tracked.ContainsKey(id) && !_saves.ContainsKey(id))
            {
                Logger.Debug("Saving update for query id {0}", id.Key);
                var obj = _tracked[id];
                obj.SetObject(results);
                _updates[id] = obj;
            }
            else
            {
                Logger.Debug("Saving new query id {0}", id.Key);
                var obj = new RiakObject(id, results);
                _saves[id] = obj;
                _tracked[id] = obj;
            }
        }

        public void DeleteQuery<TPaged>(TPaged query) where TPaged : IPaged
        {
            DeletesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(TPaged), query.ToId()));
            Logger.Debug("Deleting query id {0}", id.Key);
            _deletions.Add(id);
        }
        public void DeleteQuery<TPaged>(Action<TPaged> query) where TPaged : IPaged
        {
            var result = _mapper.CreateInstance(query);
            DeleteQuery(result);
        }


        public async Task<IEnumerable<T>> Get<T>(IEnumerable<string> keys) where T : class
        {
            ReadsMeter.Mark();
            if (!keys.Any())
                return new T[] { };

            var ids = keys.Select(key => new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(T), key)));
            var existing = _tracked.Keys.Intersect(ids);
            var except = ids.Except(existing);

            var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
            options.SetRw(Quorum.WellKnown.Quorum);

            Logger.Debug("Getting multiple objects with ids {0}", ids.Select(x => x.Key).Aggregate((cur, next) => $"{cur}, {next}"));
            if (!except.Any())
                return existing.Select(x => _tracked[x].GetObject<T>());


            var cached = new List<RiakObject>();
            var misses = new List<RiakObjectId>();
            foreach (var id in except)
            {
                var cache = MemCache.Retreive(id.ToString());
                if (cache != null)
                {
                    CacheHits.Mark();
                    cached.Add((RiakObject)cache);
                }
                CacheMisses.Mark();
                misses.Add(id);
            }

            if (misses.Any())
            {
                var results = await _client.Async.Get(misses, options).ConfigureAwait(false);
                cached.AddRange(results.Where(x => x.IsSuccess).Select(x => x.Value));
                foreach (var result in results)
                    MemCache.Cache(result.Value.ToRiakObjectId().ToString(), result.Value);
            }

            foreach (var result in cached)
                _tracked[result.ToRiakObjectId()] = result;

            //var index = 0;
            //foreach (var failed in results)
            //{
            //    // RiakResponse doesn't provide the failed Id - so pull it out of what we queried for by index and hope its the same order..
            //    if(!failed.IsSuccess)
            //        _logger.Warn("Failed to get key {1} for object {2} from riak. Error: {0}", failed.ErrorMessage, except.ElementAt(index).Key, typeof(T).FullName);
            //    index++;
            //}

            var documents = new List<T>(existing.Select(x => _tracked[x].GetObject<T>()));
            return documents.Concat(cached.Select(x =>
            {
                var obj = x?.GetObject<T>();
                return obj;
            }));
        }
        public Task<IEnumerable<T>> Get<T>(IEnumerable<ValueType> keys) where T : class
        {
            return Get<T>(keys.Select(x => x.ToString()));
        }


        public async Task<T> Get<T>(string key) where T : class
        {
            ReadsMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(T), key));

            Logger.Debug("Getting object with id {0}", id.Key);
            if (_tracked.ContainsKey(id))
                return _tracked[id].GetObject<T>();


            var cached = (RiakObject)MemCache.Retreive(id.ToString());
            if (cached != null)
            {
                CacheHits.Mark();
            }
            else
            {
                CacheMisses.Mark();

                var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
                options.SetRw(Quorum.WellKnown.Quorum);

                var result = await _client.Async.Get(id, options).ConfigureAwait(false);
                if (!result.IsSuccess)
                {
                    Logger.Warn("Failed to get key {0} for object {1} from riak - Error: {2} {3}", key,
                        typeof(T).FullName, result.ResultCode, result.ErrorMessage);
                    return default(T);
                }
                cached = result.Value;
                MemCache.Cache(id.ToString(), cached);
            }
            _tracked[id] = cached;

            var obj = cached.GetObject<T>();
            if (obj == null)
                Logger.Warn("Failed to get key {0} for object {1} from riak", key, typeof(T).FullName);

            return obj;
        }
        public Task<T> Get<T>(ValueType id) where T : class
        {
            return Get<T>(id.ToString());
        }
        public void Delete<T>(string key) where T : class
        {
            DeletesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(T), key));
            Logger.Debug("Deleting object with id {0}", id.Key);
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
            WritesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(T), key));

            if (_tracked.ContainsKey(id) && !_saves.ContainsKey(id))
            {
                Logger.Debug("Updating object with id {0}", id.Key);
                var obj = _tracked[id];
                obj.SetObject(doc);
                _updates[id] = obj;
            }
            else
            {
                Logger.Debug("Saving object with id {0}", id.Key);
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

            await _client.Async.Delete(keys).ConfigureAwait(false);
        }


        public async Task End(Exception ex = null)
        {
            if (ex != null) return;

            Logger.Debug("UOW End - {0} saves, {1} updates, {2} deletions", _saves.Count, _updates.Count, _deletions.Count);



            // New and update options commented out for now.. causing too many events to be dropped because riak doing weird things
            // - Dont comment them out, figure out riak weirdness!

            IEnumerable<RiakResult> exceptions;
            using (CommitTimer.NewContext())
            {
                var options = new RiakPutOptions
                {
                    IfNoneMatch = true,
                    IfNotModified = false,
                    ReturnHead = true,
                    ReturnBody = false
                };
                options.SetW(Quorum.WellKnown.Quorum);
                var saved = _client.Async.Put(_saves.Values, options);

                var uptOptions = new RiakPutOptions
                {
                    IfNoneMatch = false,
                    IfNotModified = true,
                    ReturnHead = true,
                    ReturnBody = false
                };
                uptOptions.SetW(Quorum.WellKnown.Quorum);
                var updated = _client.Async.Put(_updates.Values, uptOptions);

                var deleteOpt = new RiakDeleteOptions();
                deleteOpt.SetR(Quorum.WellKnown.Quorum);
                var deleted = _client.Async.Delete(_deletions, deleteOpt);

                await Task.WhenAll(saved, updated, deleted).ConfigureAwait(false);

                exceptions = saved.Result.Where(x => !x.IsSuccess)
                    .Concat(updated.Result.Where(x => !x.IsSuccess))
                    .Concat(deleted.Result.Where(x => !x.IsSuccess));
            }

            // Remove updated and deleted from cache (regardless of success)
            foreach (var update in _updates)
                MemCache.Evict(update.Key.ToString());
            foreach (var delete in _deletions)
                MemCache.Evict(delete.Key.ToString());

            NLog.MappedDiagnosticsLogicalContext.Clear();
            if (exceptions.Any())
            {
                ExceptionsMeter.Mark();
                var grouped = exceptions.GroupBy(x => x.ErrorMessage).Select(x => x.First());
                Logger.Warn("Exceptions when saving.  Details: {0}", grouped.Select(x => $"{x.ResultCode} {x.ErrorMessage}").Aggregate((cur, next) => $"{cur}, {next}"));

                foreach (var tracked in _tracked)
                    MemCache.Evict(tracked.Key.ToString());

                // Backout saves because if the event created new objects running the event again will cause more errors due to match_found
                await BackOutSaves(_saves.Keys).ConfigureAwait(false);
                throw new StorageException("Failed to commit", grouped.Select(x => x.Exception));
            }

        }
        public IEvent MutateIncoming(IEvent Event, IReadOnlyDictionary<string, string> headers)
        {
            NLog.MappedDiagnosticsLogicalContext.Clear();
            foreach (var header in headers)
                NLog.MappedDiagnosticsLogicalContext.Set(header.Key, header.Value);
            NLog.MappedDiagnosticsLogicalContext.Set("UnitOfWorkId", Guid.NewGuid());
            return Event;
        }

        public IEvent MutateOutgoing(IEvent Event)
        {
            return Event;
        }

    }
}
