
using Demo.Library.SSE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    [Flags]
    public enum ChangeType
    {
        // A new instance
        NEW = 1,
        // An instance changed
        CHANGE = 2,
        // An instance was deleted
        DELETE = 4,
        // The instance is completly different
        REFRESH = 8,
        ALL = NEW | CHANGE | DELETE | REFRESH
    }
    public interface ISubscriptionManager
    {
        Task<IEnumerable<Subscription>> GetSubscriptions();

        Task Flush();

        Task Manage(Object Payload, String QueryUrn, String SubscriptionId, ChangeType SubscriptionType, TimeSpan SubscriptionTime, String Session);
        Task Publish(Object Payload, ChangeType Type = ChangeType.CHANGE);
        Task Drop(String SubscriptionId);
        Task DropForSession(String Session);
        Task PauseForSession(String Session, Boolean Pause);
    }
}