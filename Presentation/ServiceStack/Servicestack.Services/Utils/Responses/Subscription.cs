using System;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Utils.Responses
{
    public class Subscriptions
    {
        public string Document { get; set; }
        public IEnumerable<Subscription> Sessions { get; set; }
    }

    public class Subscription
    {
        public string DocumentId { get; set; }
        public string CacheKey { get; set; }
        public DateTime Expires { get; set; }
        public string Session { get; set; }
    }
}
