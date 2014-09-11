using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.ServiceStack.Responses
{

    public class Event<T> where T : IEvent
    {
        public String Domain { get; set; }
        public String EventName { get; set; }

        public String Urn { get; set; }
        public DateTime Updated { get; set; }

        public T Payload { get; set; }
    }
}
