
using ServiceStack;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Presentation.ServiceStack.Infrastructure.Responses
{
    public class Responses_Paged<TResponse>
    {
        public IEnumerable<TResponse> Records { get; set; }

        public Int64 Total { get; set; }

        public Int64 ElapsedMs { get; set; }

        public String SubscriptionId { get; set; }

        public Int32? SubscriptionTime { get; set; }

        public String Etag { get; set; }

        public ResponseStatus ResponseStatus { get; set; }
    }

    // A pages response has just 1 subscription
    // On the client, they will receive an update when any of the paged results is updated
    // Its up to the client's viewmodel to update the correct array element
}