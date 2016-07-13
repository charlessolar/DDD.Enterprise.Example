using Aggregates;
using NServiceBus.UnitOfWork;
using Demo.Library.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Riak.Infrastructure
{
    public interface IUnitOfWork : IEventUnitOfWork
    {
        Task<IEnumerable<string>> Query<TPaged>(TPaged query) where TPaged : IPaged;
        Task<IEnumerable<string>> Query<TPaged>(Action<TPaged> query) where TPaged : IPaged;
        void SaveQuery<TPaged>(Action<TPaged> query, IEnumerable<string> results) where TPaged : IPaged;
        void SaveQuery<TPaged>(TPaged query, IEnumerable<string> results) where TPaged : IPaged;
        void DeleteQuery<TPaged>(TPaged query) where TPaged : IPaged;
        void DeleteQuery<TPaged>(Action<TPaged> query) where TPaged : IPaged;

        Task<T> Get<T>(ValueType id) where T : class;
        Task<T> Get<T>(string id) where T : class;
        Task<IEnumerable<T>> Get<T>(IEnumerable<ValueType> ids) where T : class;
        Task<IEnumerable<T>> Get<T>(IEnumerable<string> ids) where T : class;
        void Delete<T>(ValueType id) where T : class;
        void Delete<T>(string id) where T : class;
        void Delete<T>(T doc) where T : class;
        void Save<T>(T doc) where T : class;
        void Save<T>(ValueType id, T doc) where T : class;
        void Save<T>(string id, T doc) where T : class;
    }
}
