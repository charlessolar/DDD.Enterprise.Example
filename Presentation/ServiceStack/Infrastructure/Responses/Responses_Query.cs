using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using Demo.Library.SSE;
using System;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Infrastructure.Responses
{
    public class Responses_Query<TResponse>
    {
        public TResponse Payload { get; set; }
        public String SubscriptionId { get; set; }

        public Int32? SubscriptionTime { get; set; }

        public String Etag { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}