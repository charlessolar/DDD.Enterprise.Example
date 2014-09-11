using NServiceBus;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Responses
{
    public class Base
    {
        public String Status { get; set; }
        public String Message { get; set; }
    }

    public class Envelope<T> : Base
    {
        public T Payload { get; set; }
    }

}
