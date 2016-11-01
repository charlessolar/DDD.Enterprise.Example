using Aggregates;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Domain.Infrastructure
{
    public interface IUnitOfWork : ICommandUnitOfWork, IEventUnitOfWork
    {
        Task<IEnumerable<TResponse>> Query<TPaged, TResponse>(TPaged query);
        Task<IEnumerable<TResponse>> Query<TPaged, TResponse>(Action<TPaged> query);

        void SaveQuery<TPaged, TResponse>(Action<TPaged> query, IEnumerable<TResponse> results);
        void SaveQuery<TPaged, TResponse>(TPaged query, IEnumerable<TResponse> results);
        void DeleteQuery<TPaged>(TPaged query);
        void DeleteQuery<TPaged>(Action<TPaged> query);

        Task<T> Get<T>(ValueType id) where T : class;
        Task<T> Get<T>(string id) where T : class;
        void Delete<T>(ValueType id) where T : class;
        void Delete<T>(string id) where T : class;
        void Delete<T>(T doc) where T : class;
        void Save<T>(T doc) where T : class;
        void Save<T>(ValueType id, T doc) where T : class;
        void Save<T>(string id, T doc) where T : class;
    }
}
