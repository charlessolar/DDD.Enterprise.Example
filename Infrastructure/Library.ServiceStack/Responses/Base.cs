using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Responses
{
    public class Basic
    {
        public String Status { get; set; }
        public String Message { get; set; }
    }

    public class Diff<T> : Basic where T : IHasGuidId
    {
        public String Urn { get; set; }
        public Int32 Version { get; set; }

        public DateTime Updated { get; set; }

        public Object Payload { get; set; }
    }

    public class Full<T> : Basic where T : IHasGuidId
    {
        public String Urn { get; set; }
        public Int32 Version { get; set; }

        public ICollection<String> Sessions { get; set; }

        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

        public T Payload { get; set; }
    }
}
