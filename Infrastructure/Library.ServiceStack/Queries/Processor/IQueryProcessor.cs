using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries.Processor
{
    public interface IQueryProcessor
    {
        Boolean Satisfied<TResponse>(Query<TResponse> query, TResponse dto);
        Boolean Satisfied<TResponse>(PagedQuery<TResponse> query, TResponse dto);
        Responses.Query<TResponse> Process<TResponse>(Query<TResponse> query);
        Responses.Query<Responses.Paged<TResponse>> Process<TResponse>(PagedQuery<TResponse> query);
    }
}