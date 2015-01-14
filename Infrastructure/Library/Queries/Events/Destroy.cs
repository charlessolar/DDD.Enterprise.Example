using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries.Events
{
    public interface Destroy : IMessage
    {
        Guid QueryId { get; set; }
    }
}
