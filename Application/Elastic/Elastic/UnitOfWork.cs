using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NServiceBus.ObjectBuilder;
using Nest;
using Demo.Application.Elastic.Infrastructure;
using System.Threading;
using log4net;
using Demo.Application.Elastic.Infrastructure.Exceptions;
using Newtonsoft.Json;
using Metrics;
using System.Text.RegularExpressions;
using Aggregates.Contracts;
using Demo.Library.Caching;
using Elasticsearch.Net;
using NLog;
using NServiceBus;
using NServiceBus.UnitOfWork;

namespace Demo.Application.Elastic
{
    public class UnitOfWork : IUnitOfWork, IEventMutator
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public IBuilder Builder { get; set; }
        public int Retries { get; set; }

        private static readonly IntelligentCache MemCache = new IntelligentCache();
        private static readonly Meter ReadsMeter = Metric.Meter("Elastic Reads", Unit.Items);
        private static readonly Meter WritesMeter = Metric.Meter("Elastic Writes", Unit.Items);
        private static readonly Meter DeletesMeter = Metric.Meter("Elastic Deletes", Unit.Items);

        private static readonly Meter CacheHits = Metric.Meter("Elastic Cache Hits", Unit.Items);
        private static readonly Meter CacheMisses = Metric.Meter("Elastic Cache Misses", Unit.Items);

        private readonly IElasticClient _client;

        private int _pendingSize;
        private readonly BulkDescriptor _pendingDocs;
        private static object _lock = new object();
        private static bool _flush;
        private static DateTime _lastFlush = DateTime.UtcNow;

        public static int MaxBulkSize = 500;

        private class SearchResult<T> : ISearchResult<T>
        {
            public bool IsValid { get; set; }
            public IEnumerable<T> Documents { get; set; }
            public long Total { get; set; }
            public int ElapsedMs { get; set; }
        }

        public UnitOfWork(IElasticClient client)
        {
            _client = client;
            _pendingDocs = new BulkDescriptor();
        }

        public IElasticClient Raw => _client;

        public bool Flush
        {
            get { return _flush; }
            set { _flush = true; }
        }

        public async Task<bool> Exists<T>(Id id) where T : class
        {
            ReadsMeter.Mark();
            Logger.Debug("Checking if document {0} exists", id.GetString(_client.ConnectionSettings));
            return (await _client.DocumentExistsAsync<T>(id).ConfigureAwait(false)).Exists;
        }


        public async Task<ISearchResult<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            ReadsMeter.Mark();
            ISearchResponse<T> searchResult = await _client.SearchAsync<T>(selector).ConfigureAwait(false);
            if (!searchResult.IsValid) return new SearchResult<T> { IsValid = false };

            return new SearchResult<T>
            {
                IsValid = true,
                Documents = searchResult.Documents,
                Total = searchResult.Total,
                ElapsedMs = searchResult.Took
            };
        }
        public async Task<ISearchResult<T>> SearchAll<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            ReadsMeter.Mark();
            var documents = new List<T>();
            var elapsedMs = 0;
            ISearchResponse<T> searchResult = await _client.SearchAsync<T>(x =>
            {
                x = x.SearchType(SearchType.Scan);
                x = x.Scroll("4s");

                return selector(x);
            }).ConfigureAwait(false);
            if (!searchResult.IsValid) return new SearchResult<T> { IsValid = false };


            do
            {
                documents.AddRange(searchResult.Documents);
                elapsedMs += searchResult.Took;

                searchResult = await _client.ScrollAsync<T>("4s", searchResult.ScrollId).ConfigureAwait(false);

                if (!searchResult.IsValid) return new SearchResult<T> { IsValid = false };
            } while (searchResult.Documents.Any());

            return new SearchResult<T>
            {
                IsValid = true,
                Documents = documents,
                Total = searchResult.Total,
                ElapsedMs = elapsedMs
            };
        }
        public async Task<T> Get<T>(Id id) where T : class
        {
            ReadsMeter.Mark();
            Logger.Debug("Retreiving document {0}", id.GetString(_client.ConnectionSettings));

            var cached = MemCache.Retreive(id.GetString(_client.ConnectionSettings));
            if (cached != null)
            {
                CacheHits.Mark();
                return (T)cached;
            }
            CacheMisses.Mark();

            var response = await _client.GetAsync<T>(id).ConfigureAwait(false);
            if (!response.Found) return null;

            MemCache.Cache(id.GetString(_client.ConnectionSettings), response.Source);
            return response.Source;
        }
        public async Task<IEnumerable<T>> Get<T>(IEnumerable<string> ids) where T : class
        {
            ReadsMeter.Mark();
            if (!ids.Any())
                return new T[] { };

            Logger.Debug("Retreiving multiple documents {0}", ids.Aggregate((cur, next) => cur + ", " + next));

            var cached = new List<T>();
            var misses = new List<string>();
            foreach (var id in ids)
            {
                var cache = MemCache.Retreive(id);
                if (cache != null)
                {
                    CacheHits.Mark();
                    cached.Add((T)cache);
                }
                CacheMisses.Mark();
                misses.Add(id);
            }

            if (misses.Any())
            {
                var response = await _client.GetManyAsync<T>(misses).ConfigureAwait(false);
                cached.AddRange(response.Where(x => x.Found).Select(x => x.Source));
                foreach (var result in response.Where(x => x.Found))
                    MemCache.Cache(result.Id, result.Source);
            }
            return cached;
        }
        public void Index<T>(Func<BulkIndexDescriptor<T>, IIndexOperation<T>> bulkIndexSelector) where T : class
        {
            WritesMeter.Mark();
            _pendingSize++;
            _pendingDocs.Index(bulkIndexSelector);
        }

        public void Index<T>(Id id, T document) where T : class
        {
            WritesMeter.Mark();
            _pendingSize++;
            Logger.Debug("Indexing document {0}", id.GetString(_client.ConnectionSettings));
            _pendingDocs.Index<T>(x => x.Id(id).Document(document));
        }
        public void Update<T>(Id id, object document) where T : class
        {
            WritesMeter.Mark();
            _pendingSize++;
            Logger.Debug("Updating document {0}", id.GetString(_client.ConnectionSettings));
            _pendingDocs.Update<T, object>(x => x.Id(id).Doc(document));
        }
        public void Update<T>(Id id, string script, Func<FluentDictionary<string, object>, FluentDictionary<string, object>> param) where T : class
        {
            WritesMeter.Mark();
            _pendingSize++;
            Logger.Debug("Updating scripted document {0}", id.GetString(_client.ConnectionSettings));
            _pendingDocs.Update<T, object>(x => x.Id(id).Script(script).Params(param).RetriesOnConflict(5));
        }
        public void Upsert<T>(Id id, T insert, object document) where T : class
        {
            WritesMeter.Mark();
            _pendingSize++;
            Logger.Debug("Upserting document {0}", id.GetString(_client.ConnectionSettings));
            _pendingDocs.Update<T, object>(x => x.Id(id).Doc(document).Upsert(insert));
        }
        public void Upsert<T>(Id id, T insert, string script, Func<FluentDictionary<string, object>, FluentDictionary<string, object>> param) where T : class
        {
            WritesMeter.Mark();
            _pendingSize++;
            Logger.Debug("Upserting document {0}", id.GetString(_client.ConnectionSettings));
            _pendingDocs.Update<T, object>(x => x.Id(id).Script(script).Params(param).Upsert(insert).RetriesOnConflict(5));
        }
        public void Delete<T>(Id id) where T : class
        {
            DeletesMeter.Mark();
            _pendingSize++;
            Logger.Debug("Deleting document {0}", id.GetString(_client.ConnectionSettings));
            _pendingDocs.Delete<T>(x => x.Id(id));
        }

        public Task Begin()
        {
            return Task.FromResult(true);
        }

        public async Task End(Exception ex = null)
        {
            if (ex != null) return;

            if (_pendingSize > 0)
            {
                // Index the pending docs
                var response = await _client.BulkAsync(_pendingDocs.Consistency(Consistency.Quorum)).ConfigureAwait(false);
                if (response.Errors)
                {
                    foreach (var item in response.ItemsWithErrors.Select(x => x.Id))
                        MemCache.Evict(item);

                    throw new StorageException(response);
                }
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
