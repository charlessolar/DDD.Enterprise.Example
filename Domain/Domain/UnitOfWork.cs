using Aggregates.Contracts;
using Metrics;
using NLog;
using NServiceBus.MessageInterfaces;
using NServiceBus.ObjectBuilder;
using Demo.Domain.Infrastructure;
using Demo.Domain.Infrastructure.Exceptions;
using Demo.Domain.Infrastructure.Extensions;
using Demo.Domain.Infrastructure.Riak;
using RiakClient;
using RiakClient.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NServiceBus;
using Demo.Library.Caching;

namespace Demo.Domain
{
    public class UnitOfWork : IUnitOfWork, ICommandMutator, IEventMutator
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

        public async Task<IEnumerable<TResponse>> Query<TPaged, TResponse>(TPaged query)
        {
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(TPaged), query.ToId()));

            ReadsMeter.Mark();

            if (_tracked.ContainsKey(id))
                return _tracked[id].GetObject<IEnumerable<TResponse>>() ?? new TResponse[] { };

            var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
            options.SetRw(Quorum.WellKnown.Quorum);

            Logger.Debug("Getting query id {0}", id.Key);
            var result = await _client.Async.Get(id, options).ConfigureAwait(false);
            if (!result.IsSuccess)
            {
                Logger.Debug("Failed to get key {0} for query {1} from riak - Error: {2} {3}", id.Key, typeof(TPaged).FullName, result.ResultCode, result.ErrorMessage);
                return new TResponse[] { };
            }

            _tracked[id] = result.Value;

            return result.Value.GetObject<IEnumerable<TResponse>>() ?? new TResponse[] { };

        }
        public Task<IEnumerable<TResponse>> Query<TPaged, TResponse>(Action<TPaged> query)
        {
            var result = _mapper.CreateInstance(query);
            return Query<TPaged, TResponse>(result);
        }
        public void SaveQuery<TPaged, TResponse>(Action<TPaged> query, IEnumerable<TResponse> results)
        {
            var result = _mapper.CreateInstance(query);
            SaveQuery(result, results);
        }
        public void SaveQuery<TPaged, TResponse>(TPaged query, IEnumerable<TResponse> results)
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

        public void DeleteQuery<TPaged>(TPaged query)
        {
            DeletesMeter.Mark();
            var id = new RiakObjectId("default", Settings.Bucket, Settings.KeyGenerator(typeof(TPaged), query.ToId()));
            Logger.Debug("Deleting query id {0}", id.Key);
            _deletions.Add(id);
        }
        public void DeleteQuery<TPaged>(Action<TPaged> query)
        {
            var result = _mapper.CreateInstance(query);
            DeleteQuery(result);
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
                    Logger.Warn("Failed to get key {0} for object {1} from riak - Error: {2} {3}", key, typeof(T).FullName, result.ResultCode, result.ErrorMessage);
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

            if (exceptions.Any())
            {
                ExceptionsMeter.Mark();
                var grouped = exceptions.GroupBy(x => x.ErrorMessage).Select(x => x.First());
                Logger.Warn("Exceptions when saving.  Details: {0}", grouped.Select(x => $"{x.ResultCode} {x.ErrorMessage}").Aggregate((cur, next) => $"{cur}, {next}"));

                foreach (var tracked in _tracked)
                    MemCache.Evict(tracked.Key.ToString());

                // Backout saves because if the event created new objects running the event again will cause more errors due to match_found
                await BackOutSaves(_saves.Keys).ConfigureAwait(false);
                throw new StorageException("Failed to commit", exceptions.Select(x => x.Exception));
            }

        }

        public ICommand MutateIncoming(ICommand command, IReadOnlyDictionary<string, string> headers)
        {
            NLog.MappedDiagnosticsLogicalContext.Clear();
            foreach (var header in headers)
                NLog.MappedDiagnosticsLogicalContext.Set($"{Aggregates.Internal.UnitOfWork.PrefixHeader}.{header.Key}", header.Value);
            NLog.MappedDiagnosticsLogicalContext.Set("UnitOfWorkId", Guid.NewGuid());

            return command;
        }

        public ICommand MutateOutgoing(ICommand command)
        {
            return command;
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
