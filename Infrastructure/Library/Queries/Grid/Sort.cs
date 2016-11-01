using System.Runtime.Serialization;

namespace Demo.Library.Queries.Grid
{
    [DataContract]
    public class Sort
    {
        [DataMember(Name = "field")]
        public string Field { get; set; }
        [DataMember(Name = "dir")]
        public string Dir { get; set; }
    }
}
