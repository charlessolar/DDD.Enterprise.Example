using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries.Grid
{
    [DataContract]
    public class Filter
    {
        [DataMember(Name = "field")]
        public String Field { get; set; }
        [DataMember(Name = "operator")]
        public String Operator { get; set; }
        [DataMember(Name = "value")]
        public String Value { get; set; }
        [DataMember(Name = "logic")]
        public String Logic { get; set; }
        [DataMember(Name = "filters")]
        public IEnumerable<Filter> Filters { get; set; }
    }
}
