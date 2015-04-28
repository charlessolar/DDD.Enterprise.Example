using Demo.Library.Responses;
using Raven.Client;
using ServiceStack;
using System;
using System.Linq;

namespace Demo.Library.SSE
{
    public class RavenDBSubscriptionManager : ISubscriptionManager
    {
        private readonly IDocumentStore _store;
        private readonly IServerEvents _sse;

        public RavenDBSubscriptionManager(IDocumentStore store, IServerEvents sse)
        {
            _store = store;
            _sse = sse;
        }

        public void Manage<T>(String SubscriptionId, String CacheKey, String DocumentId, String Session)
        {
        }
        public void Publish<T>(String DocumentId, T Payload, ChangeType Type = ChangeType.CHANGE)
        { }

        public void AddTracked(String DocumentId, String Session, String Channel, Int32? Expires)
        {
            //using (var session = _store.OpenSession())
            //{
            //    var existing = session.Query<Subscription>().Where(x => x.DocumentId == DocumentId && x.Session == Session && x.Channel == Channel).SingleOrDefault();

            //    if (existing == null)
            //    {
            //        existing = new Subscription
            //        {
            //            DocumentId = DocumentId,
            //            Session = Session,
            //            Channel = Channel,
            //        };
            //        session.Store(existing);
            //    }

            //    existing.Expires = DateTime.UtcNow.AddSeconds(Expires ?? 3600);
            //    session.SaveChanges();
            //}
        }

        public void RemoveTracked(String Session, String Channel)
        {
            //using (var session = _store.OpenSession())
            //{
            //    var subscriptions = session.Query<Subscription>().Where(x => x.Session == Session && x.Channel == Channel).ToList();
            //    foreach (var sub in subscriptions)
            //        session.Delete(sub);
            //}
        }

        public void Publish<T>(String DocumentId, Guid ETag, T Payload, ChangeType Type) where T : IResponse
        {
            //using (var session = _store.OpenSession())
            //{
            //    var subscriptions = session.Query<Subscription>().Where(x => x.DocumentId == DocumentId).ToList();
            //    foreach (var sub in subscriptions)
            //    {
            //        _sse.NotifySession(sub.Session,
            //            "forte.Update",
            //            new Responses.Update<T>
            //            {
            //                Payload = Payload,
            //                Type = Type,
            //                Etag = ETag,
            //                SubscriptionId = sub.Channel
            //            });
            //    }
            //}

            //using (var session = _store.OpenSession())
            //{
            //    var expirations = session.Query<Subscription>().Where(x => x.Expires < DateTime.UtcNow).ToList();
            //    foreach (var x in expirations)
            //        session.Delete(x);

            //    session.SaveChanges();
            //}
        }

        /*
         * aggregateid
         * Datetime
         * Userid
         * version
         * processid
         *
         */
    }
}