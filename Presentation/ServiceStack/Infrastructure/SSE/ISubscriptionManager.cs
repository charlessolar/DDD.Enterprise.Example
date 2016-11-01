using Demo.Library.SSE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public interface ISubscriptionManager
    {
        Task<IEnumerable<Subscription>> GetSubscriptions();

        Task Flush();

        Task Manage<TDocument>(string documentId, string subscriptionId, ChangeType subscriptionType, TimeSpan subscriptionTime, string session);
        Task Manage<TResponse>(Queries.QueriesPaged<TResponse> query, string subscriptionId, ChangeType subscriptionType, TimeSpan subscriptionTime, string session);
        Task Manage(object payload, string queryUrn, string subscriptionId, ChangeType subscriptionType, TimeSpan subscriptionTime, string session);
        Task Publish(object payload, ChangeType type = ChangeType.Change);
        Task Drop(string subscriptionId);
        Task DropForSession(string session);
        Task PauseForSession(string session, bool pause);
    }
}