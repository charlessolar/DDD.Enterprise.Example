using NServiceBus;
using System;

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