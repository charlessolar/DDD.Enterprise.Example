using NServiceBus;
using Demo.Library.SSE;
using System;

namespace Demo.Library.Updates
{
    public interface IUpdate : IEvent
    {
        object Payload { get; set; }
        ChangeType ChangeType { get; set; }
        DateTime Timestamp { get; set; }
        string ETag { get; set; }
    }
}
