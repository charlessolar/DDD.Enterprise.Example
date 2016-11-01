using System;

namespace Demo.Library.SSE
{
    public class Subscription
    {
        public string Id { get { return $"{this.SubscriptionId}/{this.Document}/{this.DocumentId}"; } set { } }
        public string SubscriptionId { get; set; }
        public string Document { get; set; }
        public string DocumentId { get; set; }
        public string CacheKey { get; set; }

        public string SerializedQuery { get; set; }
        public string SerializedQueryType { get; set; }

        public DateTime Expires { get; set; }
        public ChangeType Type { get; set; }
        public string Session { get; set; }

        public bool Paused { get; set; }
        public bool Once { get; set; }
    }
}
