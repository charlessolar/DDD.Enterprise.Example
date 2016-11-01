using System.Runtime.Serialization;

namespace Demo.Library.Queries.Grid
{
    [DataContract(Name = "aggregate")]
    public class Aggregator
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }
        [DataMember(Name = "aggregate")]
        public string Aggregate { get; set; }
    }
}
