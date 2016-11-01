using Newtonsoft.Json;
using Demo.Presentation.ServiceStack.Infrastructure.Extensions;
using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using Demo.Library.Extensions;
using Demo.Library.Future;
using Demo.Library.SSE;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StructureMap;
using Demo.Library.Queries;
using System.Reflection;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public class SubscriptionManager : ISubscriptionManager
    {
        private static readonly ILog Logger = LogManager.GetLogger("SubscriptionManager");
        private readonly IServerEvents _sse;
        private readonly ICacheClient _cache;
        private readonly IFuture _future;
        private readonly ISubscriptionStorage _storage;
        private readonly IContainer _container;

        private readonly ConcurrentDictionary<string, Message> _updates = new ConcurrentDictionary<string, Message>();

        private static readonly MethodInfo _satisfiedChange = typeof(IChange<,>).GetMethod("Satisfied", BindingFlags.Public | BindingFlags.Instance);
        private static Func<object, object, object, bool> SatisfiedChange = (instance, dto, query) => (bool)_satisfiedChange.Invoke(instance, new[] { dto, query });

        public SubscriptionManager(IServerEvents sse, ICacheClient cache, IFuture future, ISubscriptionStorage storage, IContainer container)
        {
            _sse = sse;
            _cache = cache;
            _future = future;
            _storage = storage;
            _container = container;

            _future.FireRepeatedly(TimeSpan.FromSeconds(10), this.Flush, "SSE Flush");
        }
        private class Message
        {
            public DateTime Stamp { get; set; }
            public ChangeType Type { get; set; }
            public Guid Etag { get; set; }
            public object Payload { get; set; }

            public string Document { get; set; }
            public string DocumentId { get; set; }
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

            var notifies = new Dictionary<string, IList<Responses.Update>>();

            foreach (var doc in updates.GroupBy(x => x.Document))
            {
                var interested = await _storage.Retreive(x => x.Document == doc.Key).ConfigureAwait(false);
                var indirect = interested.Where(x => x.DocumentId == "");
                foreach (var indv in doc)
                {
                    var all = interested.Where(x => x.DocumentId == indv.DocumentId).ToList();

                    // Don't foreach over all the indirect because indirect contains all the open list subscriptions
                    // and the doc we are looking at might already be subscribed to which would be weeded out by the Union above
                    foreach (var paged in indirect)
                    {
                        if (all.Any(x => x.Session == paged.Session && x.SubscriptionId == paged.SubscriptionId)) continue;
                        // Deserialize the original query that generated the list of data
                        // Run the dto through the query to test if the conditions are satisfied
                        // If so, add a new subscription and notify client of new element on list

                        // Todo: using a bit of reflection to call open generic IChange.Satisfied can replace once IChange is implemented more like FluentValidation than a generic interface see PUL-5

                        var queryType = Type.GetType(paged.SerializedQueryType);
                        var query = JsonConvert.DeserializeObject(paged.SerializedQuery, queryType);

                        var pagedType = queryType.GetInterfaces().Single(x => x.IsInterface && x.GetInterfaces().Contains(typeof(IPaged)));

                        var changeType = typeof(IChange<,>).MakeGenericType(indv.Payload.GetType(), pagedType);
                        var satisfiedChange = changeType.GetMethod("Satisfied", BindingFlags.Public | BindingFlags.Instance);

                        var change = _container.TryGetInstance(changeType);
                        if (query == null || change == null || !(bool)satisfiedChange.Invoke(change, new[] { indv.Payload, query })) continue;


                        var subscription = new Subscription
                        {
                            SerializedQuery = paged.SerializedQuery,
                            SubscriptionId = paged.SubscriptionId,
                            Document = indv.Document,
                            DocumentId = indv.DocumentId,
                            CacheKey = paged.CacheKey,
                            Expires = paged.Expires,
                            Session = paged.Session,
                            Type = paged.Type
                        };
                        all.Add(subscription);
                        await _storage.Store(subscription).ConfigureAwait(false);
                    }

                    foreach (var interest in all)
                    {
                        var key = interest.Session;
                        if (!notifies.ContainsKey(key))
                            notifies[key] = new List<Responses.Update>();

                        notifies[key].Add(new Responses.Update
                        {
                            Payload = indv.Payload,
                            Etag = indv.Etag,
                            Stamp = indv.Stamp,
                            SubscriptionId = interest.SubscriptionId,
                            Type = indv.Type
                        });
                    }
                }

                // Delete entries from cache (new data == old data stale)
                var keys = interested.Select(x => x.CacheKey).Distinct();
                if (keys.Count() != 0)
                    _cache.RemoveAll(keys.ToArray());
            }

            foreach (var job in notifies)
            {
                if (Logger.IsDebugEnabled)
                    Logger.DebugFormat("Notifying session {0} of {1} values", job.Key, job.Value.Count);
                _sse.NotifySession(job.Key, "Demo.Update", job.Value.ToArray());
            }

            await _storage.Clean().ConfigureAwait(false);
        }

        public async Task Drop(string subscriptionId)
        {
            //_logger.DebugFormat("Dropping subscription for id {0}", SubscriptionId);
            var existing = await _storage.Retreive(x => x.SubscriptionId == subscriptionId).ConfigureAwait(false);

            foreach (var x in existing)
                await _storage.Remove(x).ConfigureAwait(false);

        }
        public async Task DropForSession(string session)
        {
            //_logger.DebugFormat("Dropping subscription for session {0}", Session);
            var existing = await _storage.Retreive(x => x.Session == session).ConfigureAwait(false);

            foreach (var x in existing)
                await _storage.Remove(x).ConfigureAwait(false);
        }
        public async Task PauseForSession(string session, bool pause)
        {
            //_logger.DebugFormat("Pausing subscription for session {0}", Session);
            var existing = await _storage.Retreive(x => x.Session == session).ConfigureAwait(false);

            foreach (var x in existing)
                await _storage.Pause(x, pause).ConfigureAwait(false);
        }

        public async Task Manage(object payload, string queryUrn, string subscriptionId, ChangeType subscriptionType, TimeSpan subscriptionTime, string session)
        {
            var document = payload.GetType().FullName;

            var idField = payload.GetType().GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (idField != null)
            {
                await _storage.Store(new Subscription
                {
                    SubscriptionId = subscriptionId,
                    Document = document,
                    DocumentId = idField.GetValue(payload).ToString(),
                    CacheKey = queryUrn,
                    Expires = DateTime.UtcNow.Add(subscriptionTime),
                    Session = session,
                    Type = subscriptionType
                }).ConfigureAwait(false);
            }

        }
        public async Task Manage<TResponse>(QueriesPaged<TResponse> query, string subscriptionId, ChangeType subscriptionType, TimeSpan subscriptionTime, string session)
        {
            var key = query.GetCacheKey();
            var subscription = new Subscription
            {
                SerializedQuery = query.SerializeToString(),
                SerializedQueryType = query.GetType().AssemblyQualifiedName,
                SubscriptionId = subscriptionId,
                Document = typeof(TResponse).FullName,
                DocumentId = "",
                CacheKey = key,
                Expires = DateTime.UtcNow.Add(subscriptionTime),
                Session = session,
                Type = subscriptionType
            };

            await _storage.Store(subscription).ConfigureAwait(false);
        }
        public async Task Manage<TDocument>(string documentId, string subscriptionId, ChangeType subscriptionType, TimeSpan subscriptionTime, string session)
        {
            var subscription = new Subscription
            {
                SubscriptionId = subscriptionId,
                Document = typeof(TDocument).FullName,
                DocumentId = documentId,
                Expires = DateTime.UtcNow.Add(subscriptionTime),
                Session = session,
                Type = subscriptionType
            };

            await _storage.Store(subscription).ConfigureAwait(false);
        }

        private async Task<IEnumerable<Subscription>> RetrieveInterested(string document, string documentId)
        {

            var interested = await _storage.Retreive(x => x.Document == document).ConfigureAwait(false);

            var direct = interested.Where(x => x.DocumentId == documentId);
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
                    Document = document,
                    DocumentId = documentId,
                    CacheKey = x.CacheKey,
                    Expires = x.Expires,
                    Session = x.Session,
                    Type = x.Type
                };
                await _storage.Store(subscription).ConfigureAwait(false);
                //var paged = x as PagedSubscription<T>;
                //if (paged != null && paged.Query.SubscriptionType.HasValue && paged.Query.SubscriptionType.Value.HasFlag(ChangeType.NEW))
                //    Manage(Payload, paged.Query, paged.Session);
            }
            //}

            return all;
        }

        public Task Publish(object payload, ChangeType type = ChangeType.Change)
        {
            var document = payload.GetType().FullName;
            var idField = payload.GetType().GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (idField == null)
                return Task.FromResult(0);

            var documentId = idField.GetValue(payload).ToString();

            var msg = new Message
            {
                Document = document,
                DocumentId = documentId,
                Stamp = DateTime.UtcNow,
                Type = type,
                Payload = payload
            };
            _updates.AddOrUpdate($"{document}:{documentId}:{type}", msg, (_, x) => msg);
            

            return Task.FromResult(0);
        }
    }
}