using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Command
{
    public interface IDemoEvent : IEvent
    {
        String UserId { get; set; }
        Int64 Timestamp { get; set; }
    }
}
