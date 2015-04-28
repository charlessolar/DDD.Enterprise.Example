using Demo.Library.SSE;
using NServiceBus;
using ServiceStack;
using System;

namespace Demo.Library.Queries
{
    public class Query<TResponse> : IReturn<Responses.Query<TResponse>>
    {
        public String SubscriptionId { get; set; }

        public Int32? SubscriptionTime { get; set; }

        public ChangeType? SubscriptionType { get; set; }
    }
}