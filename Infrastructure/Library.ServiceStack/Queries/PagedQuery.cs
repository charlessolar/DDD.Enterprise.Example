using Demo.Library.Queries.Grid;
using ServiceStack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Demo.Library.Queries
{
    [DataContract]
    public class PagedQuery<TResponse> : Query<Responses.Paged<TResponse>>
    {
        [DataMember(Name = "skip", IsRequired = true)]
        public Int32? Skip { get; set; }

        [DataMember(Name = "take", IsRequired = true)]
        public Int32? Take { get; set; }

        [DataMember(Name = "sort")]
        public IEnumerable<Sort> Sort { get; set; }

        [DataMember(Name = "aggregates")]
        public IEnumerable<Aggregator> Aggregates { get; set; }

        [DataMember(Name = "filter")]
        public Filter Filter { get; set; }
    }
}