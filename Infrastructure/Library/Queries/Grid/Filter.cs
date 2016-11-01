using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Demo.Library.Queries.Grid
{
    [DataContract]
    public class Filter
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }
        [DataMember(Name = "operator")]
        public string Operator { get; set; }
        [DataMember(Name = "value")]
        public string Value { get; set; }
        [DataMember(Name = "logic")]
        public string Logic { get; set; }
        [DataMember(Name = "filters")]
        public IEnumerable<Filter> Filters { get; set; }
    }
}
