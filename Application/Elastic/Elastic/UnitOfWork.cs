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
using NLog;
using NServiceBus.UnitOfWork;

namespace Demo.Application.Elastic
{
    public class UnitOfWork : IUnitOfWork, IEventUnitOfWork
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        public IBuilder Builder { get; set; }

        private static Meter _readsMeter = Metric.Meter("Elastic Reads", Unit.Items);
        private static Meter _writesMeter = Metric.Meter("Elastic Writes", Unit.Items);
        private static Meter _deletesMeter = Metric.Meter("Elastic Deletes", Unit.Items);

        private readonly IElasticClient _client;

        private Int32 _pendingSize;
        private BulkDescriptor _pendingDocs;
        private static Object _lock = new object();
        private static Boolean _flush;
        private static DateTime _lastFlush = DateTime.UtcNow;

        public static Int32 MaxBulkSize = 500;

        private class SearchResult<T> : ISearchResult<T>
        {
            public Boolean IsValid { get; set; }
            public IEnumerable<T> Documents { get; set; }
            public Int64 Total { get; set; }
            public Int32 ElapsedMs { get; set; }
        }

        public UnitOfWork(IElasticClient client)
        {
            _client = client;
            _pendingDocs = new BulkDescriptor();
        }

        public IElasticClient Raw { get { return _client; } }
        public Boolean Flush
        {
            get { return _flush; }
            set { _flush = true; }
        }

        public async Task<Boolean> Exists<T>(Id id) where T : class
        {
            _readsMeter.Mark();
            return (await _client.DocumentExistsAsync<T>(id)).Exists;
        }


        public async Task<ISearchResult<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class
        {
            _readsMeter.Mark();
            ISearchResponse<T> searchResult = await _client.SearchAsync<T>(x =>
            {
                return selector(x);
            });
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
            _readsMeter.Mark();
            var documents = new List<T>();
            var elapsedMs = 0;
            ISearchResponse<T> searchResult = await _client.SearchAsync<T>(x =>
            {
                x = x.SearchType(Elasticsearch.Net.SearchType.Scan);
                x = x.Scroll("4s");

                return selector(x);
            });
            if (!searchResult.IsValid) return new SearchResult<T> { IsValid = false };


            do
            {
                documents.AddRange(searchResult.Documents);
                elapsedMs += searchResult.Took;

                searchResult = await _client.ScrollAsync<T>("4s", searchResult.ScrollId);

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
            _readsMeter.Mark();
            var response = await _client.GetAsync<T>(id);
            if (!response.Found) return null;

            return response.Source;
        }
        public async Task<IEnumerable<T>> Get<T>(IEnumerable<string> ids) where T : class
        {
            _readsMeter.Mark();
            
            var response = await _client.GetManyAsync<T>(ids);

            return response.Where(x => x.Found).Select(x => x.Source);
        }
        public void Index<T>(Func<BulkIndexDescriptor<T>, IIndexOperation<T>> bulkIndexSelector) where T : class
        {
            _writesMeter.Mark();
            _pendingSize++;
            _pendingDocs.Index(bulkIndexSelector);
        }

        public void Index<T>(Id id, T document) where T : class
        {
            _writesMeter.Mark();
            _pendingSize++;
            _pendingDocs.Index<T>(x => x.Id(id).Document(document));
        }
        public void Update<T>(Id id, Object document) where T : class
        {
            _writesMeter.Mark();
            _pendingSize++;
            _pendingDocs.Update<T, object>(x => x.Id(id).Doc(document));
        }
        public void Update<T>(Id id, String script, Func<FluentDictionary<String,object>, FluentDictionary<String,object>> param) where T : class
        {
            _writesMeter.Mark();
            _pendingSize++;
            _pendingDocs.Update<T, object>(x => x.Id(id).Script(script).Params(param));
        }

        public Task Begin()
        {
            return Task.FromResult(true);
        }

        public async Task End(Exception ex = null)
        {
            if (ex != null)
            {
                return;
            }

            if (_pendingSize > 0)
            {
                // Index the pending docs
                var response = await _client.BulkAsync(_pendingDocs.Consistency(Elasticsearch.Net.Consistency.Quorum));
                if (response.Errors)
                    throw new StorageException(response);
            }

        }
    }
}
