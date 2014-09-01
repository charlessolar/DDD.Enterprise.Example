using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Responses
{
    public class Base<T> where T : IHasGuidId
    {
        public String Urn { get; set; }
        public Int32 Version { get; set; }
        public T Payload { get; set; }
    }
}
