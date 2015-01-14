using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries.Events
{
    public interface Update : IMessage
    {
        Guid QueryId { get; set; }

        Int32 Version { get; set; }

        Object Payload { get; set; }
    }
}
