using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using Demo.Library.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.Responses
{

    public class Update
    {
        public Object Payload { get; set; }

        public String SubscriptionId { get; set; }

        public ChangeType Type { get; set; }

        public Guid Etag { get; set; }
        public DateTime Stamp { get; set; }
    }
}