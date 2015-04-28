using StructureMap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries.Processor
{
    public sealed class QueryProcessor : IQueryProcessor
    {
        private readonly IContainer _container;

        public QueryProcessor(IContainer container)
        {
            this._container = container;
        }

        [DebuggerStepThrough]
        public Boolean Satisfied<TResponse>(Query<TResponse> query, TResponse dto)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));

            dynamic handler = _container.GetInstance(handlerType);

            return handler.Satisfied((dynamic)query, (dynamic)dto);
        }

        [DebuggerStepThrough]
        public Boolean Satisfied<TResponse>(PagedQuery<TResponse> query, TResponse dto)
        {
            var handlerType = typeof(IPagingQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));

            dynamic handler = _container.GetInstance(handlerType);

            return handler.Satisfied((dynamic)query, (dynamic)dto);
        }

        [DebuggerStepThrough]
        public Responses.Query<TResponse> Process<TResponse>(Query<TResponse> query)
        {
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));

            dynamic handler = this._container.GetInstance(handlerType);

            return handler.Handle((dynamic)query);
        }
        [DebuggerStepThrough]
        public Responses.Query<Responses.Paged<TResponse>> Process<TResponse>(PagedQuery<TResponse> query)
        {
            var handlerType = typeof(IPagingQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResponse));

            dynamic handler = this._container.GetInstance(handlerType);

            return handler.Handle((dynamic)query);
        }
    }
}