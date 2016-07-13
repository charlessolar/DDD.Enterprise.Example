using Demo.Library.SSE;
using NServiceBus;
using ServiceStack;
using System;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using Demo.Presentation.ServiceStack.Infrastructure.Queries;

namespace Demo.Presentation.ServiceStack.Infrastructure.Queries
{
    public class Queries_Query<TResponse> : Library.Queries.IQuery, IReturn<Responses.Responses_Query<TResponse>>
    {
        public String SubscriptionId { get; set; }

        public Int32? SubscriptionTime { get; set; }

        public ChangeType? SubscriptionType { get; set; }
    }
}