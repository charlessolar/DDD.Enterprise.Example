using Demo.Presentation.ServiceStack.Infrastructure.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Utils.Responses
{
    public class Subscriptions
    {
        public String Document { get; set; }
        public IEnumerable<Subscription> Sessions { get; set; }
    }

    public class Subscription
    {
        public String DocumentId { get; set; }
        public String CacheKey { get; set; }
        public DateTime Expires { get; set; }
        public String Session { get; set; }
    }
}
