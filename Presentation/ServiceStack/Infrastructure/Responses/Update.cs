using Demo.Library.SSE;
using System;

namespace Demo.Presentation.ServiceStack.Infrastructure.Responses
{

    public class Update
    {
        public object Payload { get; set; }

        public string SubscriptionId { get; set; }

        public ChangeType Type { get; set; }

        public Guid Etag { get; set; }
        public DateTime Stamp { get; set; }
    }
}