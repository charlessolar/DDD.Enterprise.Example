using NServiceBus;
using System;
using System.Collections.Generic;

namespace Demo.Library.Queries
{
    public class Result : IMessage
    {
        public IEnumerable<Object> Records { get; set; }
    }
}