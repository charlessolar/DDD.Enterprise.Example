using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries
{
    public interface IHandleQueries<TQuery> : IHandleMessagesAsync<TQuery> where TQuery : IQuery
    {
    }
}
