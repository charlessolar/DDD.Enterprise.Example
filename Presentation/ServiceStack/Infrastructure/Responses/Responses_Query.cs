using ServiceStack;

namespace Demo.Presentation.ServiceStack.Infrastructure.Responses
{
    public class ResponsesQuery<TResponse>
    {
        public TResponse Payload { get; set; }
        public string SubscriptionId { get; set; }

        public int? SubscriptionTime { get; set; }

        public string Etag { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }
}