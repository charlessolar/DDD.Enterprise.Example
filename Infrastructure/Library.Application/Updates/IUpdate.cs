using NServiceBus;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Updates
{
    public interface Update : IMessage
    {
        Object Payload { get; set; }
        ChangeType ChangeType { get; set; }
        DateTime Timestamp { get; set; }
        String ETag { get; set; }
    }
}
