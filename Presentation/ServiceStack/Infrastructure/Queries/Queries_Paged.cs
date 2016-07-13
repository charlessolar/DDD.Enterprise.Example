
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Demo.Library.Queries;
using Newtonsoft.Json;
using System.Linq;
using Demo.Library.Queries.Grid;
using ServiceStack;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;

namespace Demo.Presentation.ServiceStack.Infrastructure.Queries
{
    [DataContract]
    public class Queries_Paged<TResponse> : IReturn<Responses.Responses_Paged<TResponse>>, IPaged
    {
        [DataMember(Name = "skip", IsRequired = true)]
        public Int32? Skip { get; set; }

        [DataMember(Name = "take", IsRequired = true)]
        public Int32? Take { get; set; }
        
        [DataMember(Name = "sort", IsRequired = true)]
        public IEnumerable<Sort> Sort { get; set; }
        
        [DataMember(Name = "aggregates")]
        public IEnumerable<Aggregator> Aggregates { get; set; }

        [DataMember(Name = "filter")]
        public Filter Filter { get; set; }


        [DataMember(Name = "page")]
        public Int32? Page { get; set; }

        [DataMember(Name = "pagesize")]
        public Int32? PageSize { get; set; }


        [DataMember(Name = "SubscriptionId")]
        public String SubscriptionId { get; set; }

        [DataMember(Name = "SubscriptionTime")]
        public Int32? SubscriptionTime { get; set; }

        [DataMember(Name = "SubscriptionType")]
        public ChangeType? SubscriptionType { get; set; }

    }
}