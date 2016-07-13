using Newtonsoft.Json;
using Demo.Presentation.ServiceStack.Infrastructure.Extensions;
using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using Demo.Library.Extensions;
using Demo.Library.Future;
using Demo.Library.SSE;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Logging;
using ServiceStack.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private static ILog _logger = LogManager.GetLogger("SubscriptionManager");
        private readonly IServerEvents _sse;
        private readonly ICacheClient _cache;
        private readonly IFuture _future;
        private readonly ISubscriptionStorage _storage;

        private ConcurrentDictionary<String, Message> _updates = new ConcurrentDictionary<String, Message>();

        public SubscriptionManager(IServerEvents sse, ICacheClient cache, IFuture future, ISubscriptionStorage storage)
        {
            _sse = sse;
            _cache = cache;
            _future = future;
            _storage = storage;

            _future.FireRepeatedly(TimeSpan.FromSeconds(10), this.Flush, "SSE Flush");
        }
        private class Message
        {
            public DateTime Stamp { get; set; }
            public ChangeType Type { get; set; }
            public Guid Etag { get; set; }
            public Object Payload { get; set; }

            public String Document { get; set; }
            public String DocumentId { get; set; }
        }

        public Task<IEnumerable<Subscription>> GetSubscriptions()
        {
            return _storage.Retreive((_) => true);
        }

        public async Task Flush()
        {
            // Copy and empty the bag
            var updates = _updates.Values.ToList();
            _updates.Clear();

            
            var notifies = new Dictionary<String, IList<Responses.Update>>();

            
            foreach (var doc in updates)
            {
                var interested = await RetrieveInterested(doc.Document, doc.DocumentId);

                foreach(var interest in interested.Distinct(x => new { x.Session, x.SubscriptionId }))
                {
                    if (!notifies.ContainsKey(interest.Session))
                        notifies[interest.Session] = new List<Responses.Update>();

                    notifies[interest.Session].Add(new Responses.Update
                    {
                        Payload = doc.Payload,
                        Etag = doc.Etag,
                        Stamp = doc.Stamp,
                        SubscriptionId = interest.SubscriptionId,
                        Type = doc.Type
                    });
                }

                // Delete entries from cache (new data == old data stale)
                var keys = interested.Select(x => x.CacheKey).Distinct();
                if (keys.Count() != 0)
                    _cache.RemoveAll(keys.ToArray());
            }

            foreach (var job in notifies)
            {
                if (_logger.IsDebugEnabled)
                    _logger.DebugFormat("Notifying session {0} of {1} values", job.Key, job.Value.Count);
                _sse.NotifySession(job.Key, "Demo.Update", job.Value.ToArray());
            }

            await _storage.Clean();
        }

        public async Task Drop(String SubscriptionId)
        {
            //_logger.DebugFormat("Dropping subscription for id {0}", SubscriptionId);
            var existing = await _storage.Retreive(x => x.SubscriptionId == SubscriptionId);

            foreach (var x in existing)
                await _storage.Remove(x);

        }
        public async Task DropForSession(String Session)
        {
            //_logger.DebugFormat("Dropping subscription for session {0}", Session);
            var existing = await _storage.Retreive(x => x.Session == Session);

            foreach (var x in existing)
                await _storage.Remove(x);
        }
        public async Task PauseForSession(String Session, Boolean Pause)
        {
            //_logger.DebugFormat("Pausing subscription for session {0}", Session);
            var existing = await _storage.Retreive(x => x.Session == Session);

            foreach (var x in existing)
                await _storage.Pause(x, Pause);
        }

        public async Task Manage(Object Payload, String QueryUrn, String SubscriptionId, ChangeType SubscriptionType, TimeSpan SubscriptionTime, String Session)
        {
            var document = Payload.GetType().FullName;

            var idField = Payload.GetType().GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (idField != null)
            {
                await _storage.Store(new Subscription
                {
                    SubscriptionId = SubscriptionId,
                    Document = document,
                    DocumentId = idField.GetValue(Payload).ToString(),
                    CacheKey = QueryUrn,
                    Expires = DateTime.UtcNow.Add(SubscriptionTime),
                    Session = Session,
                    Type = SubscriptionType
                });                
            }

            var subscription = new Subscription
            {
                SubscriptionId = SubscriptionId,
                Document = document,
                DocumentId = "",
                CacheKey = QueryUrn,
                Expires = DateTime.UtcNow.Add(SubscriptionTime),
                Session = Session,
                Type = SubscriptionType
            };

            await _storage.Store(subscription);
        }

        private async Task<IEnumerable<Subscription>> RetrieveInterested(String Document, String DocumentId)
        {

            var interested = await _storage.Retreive(x => x.Document == Document);

            var direct = interested.Where(x => x.DocumentId == DocumentId);


            var indirect = interested.Where(x => x.DocumentId == "");

            var all = direct.Union(indirect, x => x.SubscriptionId);


            //var all = direct.Concat(indirect).Distinct(x => new { x.DocumentId, x.SubscriptionId });

            //if (Type == ChangeType.NEW)
            //{
            // If document is new, add it to all direct subscriptions which have NEW change type
            foreach (var x in indirect)
            {
                var subscription = new Subscription
                {
                    SubscriptionId = x.SubscriptionId,
                    Document = Document,
                    DocumentId = DocumentId,
                    CacheKey = x.CacheKey,
                    Expires = x.Expires,
                    Session = x.Session,
                    Type = x.Type
                };
                await _storage.Store(subscription);
                //var paged = x as PagedSubscription<T>;
                //if (paged != null && paged.Query.SubscriptionType.HasValue && paged.Query.SubscriptionType.Value.HasFlag(ChangeType.NEW))
                //    Manage(Payload, paged.Query, paged.Session);
            }
            //}

            return all;
        }

        public Task Publish(Object Payload, ChangeType Type = ChangeType.CHANGE)
        {
            var document = Payload.GetType().FullName;
            var idField = Payload.GetType().GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (idField == null)
                return Task.FromResult(0);

            var documentId = idField.GetValue(Payload).ToString();

            
            _updates[$"{document}:{documentId}:{Type}"] = new Message
            {
                Document = document,
                DocumentId = documentId,
                Stamp = DateTime.UtcNow,
                Type = Type,
                Payload = Payload
            };

            return Task.FromResult(0);
        }
    }
}