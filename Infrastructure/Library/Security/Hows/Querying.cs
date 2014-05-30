using Demo.Library.Queries;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Hows
{
    // Happens when pulling data from something - used for row-level filtering
    public class Querying : IHow, IHandleMessages<BasicQuery>
    {
        public String Description { get { return "Query"; } }

        public Querying()
        {
        }
        public void Handle(BasicQuery message)
        {
            //var result = _manager.Authorize(message);
            //if (!result.IsAuthorized)
            //    _bus.DoNotContinueDispatchingCurrentMessageToHandlers();
        }
    }
}