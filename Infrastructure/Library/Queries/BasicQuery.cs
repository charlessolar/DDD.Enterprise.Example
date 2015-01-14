using NServiceBus;
using System;

namespace Demo.Library.Queries
{
    public class BasicQuery : IMessage
    {
        Guid QueryId { get; set; }
        Int32? Timeout { get; set; }
    }
}