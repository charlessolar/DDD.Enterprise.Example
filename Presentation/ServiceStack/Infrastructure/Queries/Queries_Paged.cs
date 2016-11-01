using System.Collections.Generic;
using System.Runtime.Serialization;
using Demo.Library.Queries;
using Demo.Library.Queries.Grid;
using ServiceStack;
using Demo.Library.SSE;

namespace Demo.Presentation.ServiceStack.Infrastructure.Queries
{
    [DataContract]
    public class QueriesPaged<TResponse> : IReturn<Responses.ResponsesPaged<TResponse>>, IPaged
    {
        [DataMember(Name = "skip", IsRequired = true)]
        public int? Skip { get; set; }

        [DataMember(Name = "take", IsRequired = true)]
        public int? Take { get; set; }
        
        [DataMember(Name = "sort", IsRequired = true)]
        public IEnumerable<Sort> Sort { get; set; }
        
        [DataMember(Name = "aggregates")]
        public IEnumerable<Aggregator> Aggregates { get; set; }

        [DataMember(Name = "filter")]
        public Filter Filter { get; set; }


        [DataMember(Name = "page")]
        public int? Page { get; set; }

        [DataMember(Name = "pagesize")]
        public int? PageSize { get; set; }


        [DataMember(Name = "SubscriptionId")]
        public string SubscriptionId { get; set; }

        [DataMember(Name = "SubscriptionTime")]
        public int? SubscriptionTime { get; set; }

        [DataMember(Name = "SubscriptionType")]
        public ChangeType? SubscriptionType { get; set; }

    }
}