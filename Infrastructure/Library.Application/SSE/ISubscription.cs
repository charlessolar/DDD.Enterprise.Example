using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.SSE
{
    public class Subscription
    {
        public String Id { get { return $"{this.SubscriptionId}/{this.Document}/{this.DocumentId}"; } set { } }
        public String SubscriptionId { get; set; }
        public String Document { get; set; }
        public String DocumentId { get; set; }
        public String CacheKey { get; set; }

        public DateTime Expires { get; set; }
        public ChangeType Type { get; set; }
        public String Session { get; set; }

        public Boolean Paused { get; set; }
    }
}
