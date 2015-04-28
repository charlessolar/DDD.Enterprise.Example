using Demo.Library.SSE;
using ServiceStack;
using System;

namespace Demo.Library.Responses
{
    public class Query<T> : Base<T>
    {
        public String SubscriptionId { get; set; }

        public Int32? SubscriptionTime { get; set; }

        public Guid? Etag { get; set; }
    }
}