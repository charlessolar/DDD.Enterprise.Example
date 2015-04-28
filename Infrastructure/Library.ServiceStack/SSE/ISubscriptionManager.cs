using Demo.Library.Responses;
using Raven.Abstractions.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Library.SSE
{
    public interface ISubscriptionManager
    {
        void Manage<T>(String CacheKey, String DocumentId, String SubscriptionId = "", String Session = "");
        void Publish<T>(String DocumentId, T Payload, ChangeType Type = ChangeType.CHANGE);
    }
}