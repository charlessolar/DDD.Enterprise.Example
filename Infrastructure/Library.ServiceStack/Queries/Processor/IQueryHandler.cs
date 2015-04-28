using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q = Demo.Library.Queries;
using R = Demo.Library.Responses;

namespace Demo.Library.Queries.Processor
{
    public interface IQueryHandler<TQuery, TResponse>
        where TQuery : Query<TResponse>
    {
        Boolean Satisfied(TQuery query, TResponse dto);
        R.Query<TResponse> Handle(TQuery query);
    }

    public interface IPagingQueryHandler<TQuery, TResponse>
        where TQuery : PagedQuery<TResponse>
    {
        Boolean Satisfied(TQuery query, TResponse dto);
        R.Query<R.Paged<TResponse>> Handle(TQuery query);
    }
}