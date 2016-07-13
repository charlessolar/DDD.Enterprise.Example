using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Queries.Grid
{
    [DataContract(Name = "aggregate")]
    public class Aggregator
    {
        [DataMember(Name = "field")]
        public String Field { get; set; }
        [DataMember(Name = "aggregate")]
        public String Aggregate { get; set; }
    }
}
