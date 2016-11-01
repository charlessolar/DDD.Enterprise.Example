
using ServiceStack;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Infrastructure.Responses
{
    public class ResponsesPaged<TResponse>
    {
        public IEnumerable<TResponse> Records { get; set; }

        public long Total { get; set; }

        public long ElapsedMs { get; set; }

        public string SubscriptionId { get; set; }

        public int? SubscriptionTime { get; set; }

        public string Etag { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    // A pages response has just 1 subscription
    // On the client, they will receive an update when any of the paged results is updated
    // Its up to the client's viewmodel to update the correct array element
}