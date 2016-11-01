using Demo.Library.SSE;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Infrastructure.Queries
{
    public class QueriesQuery<TResponse> : Library.Queries.IQuery, IReturn<Responses.ResponsesQuery<TResponse>>
    {
        public string SubscriptionId { get; set; }

        public int? SubscriptionTime { get; set; }

        public ChangeType? SubscriptionType { get; set; }
    }
}