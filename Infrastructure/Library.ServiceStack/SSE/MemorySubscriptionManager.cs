using Demo.Library.Extensions;
using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.SSE
{
    public class MemorySubscriptionManager : ISubscriptionManager
    {
        private readonly IServerEvents _sse;
        private readonly ICacheClient _cache;
        private static IList<Subscription> _subscriptions = new List<Subscription>();

        public MemorySubscriptionManager(IServerEvents sse, ICacheClient cache)
        {
            _sse = sse;
            _cache = cache;
        }

        public void Manage<T>(String CacheKey, String DocumentId, String SubscriptionId, String Session)
        {
            var existing = _subscriptions.SingleOrDefault(x => x.SubscriptionId == SubscriptionId && x.DocumentId == DocumentId && x.Session == Session);

            if (existing != null) return;

            _subscriptions.Add(new Subscription
            {
                SubscriptionId = SubscriptionId,
                CacheKey = CacheKey,
                Document = typeof(T).FullName,
                DocumentId = DocumentId,
                Session = Session
            });
        }


        public void Publish<T>(String DocumentId, T Payload, ChangeType Type = ChangeType.CHANGE)
        {
            var interested = _subscriptions.Where(x => x.DocumentId == DocumentId && x.Document == typeof(T).FullName);

            if (Type == ChangeType.NEW)
            {
                var interestedNew = _subscriptions.Where(x => x.Document == typeof(T).FullName && x.DocumentId.IsNullOrEmpty());

                foreach (var sub in interestedNew)
                    Manage<T>(sub.CacheKey, DocumentId, sub.SubscriptionId, sub.Session);

                interested = interested.Concat(interestedNew);
            }
            interested = interested.Distinct(x => x.SubscriptionId);

            // Delete entries from cache (new data == old data stale)
            _cache.RemoveAll(interested.Select(x => x.CacheKey).Distinct());

            foreach (var sub in interested)
            {
                _sse.NotifySession(sub.Session,
                        "forte.Update",
                        new Responses.Update<T>
                        {
                            Payload = Payload,
                            Type = Type,
                            //Etag = ETag,
                            SubscriptionId = sub.SubscriptionId
                        });
            }
        }
    }
}