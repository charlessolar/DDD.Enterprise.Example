using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.Library.SSE
{
    public interface ISubscriptionManager
    {
        void AddTracked(String Session, String Receiver, Guid QueryId, Int32? Timeout);
        void RemoveTracked(String Session, Guid QueryId);

        void Publish(Guid QueryId, Int32 Version, Object Payload);
    }
}