using Demo.Library.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.SSE
{
    public enum ChangeType
    {
        NEW,
        CHANGE,
        DELETE
    }

    public class Subscription
    {
        public String SubscriptionId { get; set; }

        public String Document { get; set; }
        public String DocumentId { get; set; }

        public String CacheKey { get; set; }


        public String Session { get; set; }
    }
}