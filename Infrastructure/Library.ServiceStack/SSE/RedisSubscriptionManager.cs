using System;
using System.Collections.Generic;

namespace Demo.Infrastructure.Library.SSE
{
    public class RedisSubscriptionManager : ISubscriptionManager
    {
        public void AddTracked(string Session, string ReceiverId, Guid QueryId, int? Timeout)
        {
            throw new NotImplementedException();
        }

        public void RemoveTracked(string Session, Guid QueryId)
        {
            throw new NotImplementedException();
        }

        public void Publish(Guid QueryId, int Version, object Payload)
        {
            throw new NotImplementedException();
        }
    }
}