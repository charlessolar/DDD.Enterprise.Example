using Demo.Library.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Responses
{
    public class Update<T>
    {
        public T Payload { get; set; }

        public String SubscriptionId { get; set; }

        public ChangeType Type { get; set; }

        public Guid Etag { get; set; }
    }
}