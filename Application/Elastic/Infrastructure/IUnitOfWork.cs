using Aggregates;
using Nest;
using NServiceBus.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Elastic.Infrastructure
{
    public interface ISearchResult<T>
    {
        Boolean IsValid { get; }
        IEnumerable<T> Documents { get; }
        Int64 Total { get; }
        Int32 ElapsedMs { get; }
    }
    public interface IUnitOfWork : IEventUnitOfWork
    {
        IElasticClient Raw { get; }
        Task<ISearchResult<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class;
        Task<ISearchResult<T>> SearchAll<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class;
        void Index<T>(Func<BulkIndexDescriptor<T>, IIndexOperation<T>> bulkIndexSelector) where T : class;
        void Index<T>(Id id, T document) where T : class;
        void Update<T>(Id id, Object document) where T : class;
        void Update<T>(Id id, String script, Func<FluentDictionary<String, object>, FluentDictionary<String, object>> param) where T : class;
        Task<T> Get<T>(Id id) where T : class;
        Task<IEnumerable<T>> Get<T>(IEnumerable<string> ids) where T : class;
        Task<Boolean> Exists<T>(Id id) where T : class;
    }
}
