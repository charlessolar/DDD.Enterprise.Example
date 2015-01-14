using ServiceStack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Demo.Infrastructure.Library.SSE
{
    class Listener
    {
        public String Receiver { get; set; }
        public DateTime? Timeout { get; set; }
    }
    public class MemorySubscriptionManager : ISubscriptionManager
    {
        private readonly IServerEvents _sse;
        private ConcurrentDictionary<Guid, IDictionary<String, Listener>> _subscriptions;

        public MemorySubscriptionManager(IServerEvents sse)
        {
            _sse = sse;
            _subscriptions = new ConcurrentDictionary<Guid, IDictionary<String, Listener>>();
        }

        public void AddTracked(String Session, String Receiver, Guid QueryId, Int32? Timeout)
        {
            var listener = new Listener { Receiver = Receiver, Timeout = Timeout.HasValue ? DateTime.UtcNow.AddSeconds( Timeout.Value ) : (DateTime?)null };

            _subscriptions.AddOrUpdate(
                QueryId,
                new Dictionary<String, Listener> { { Session, listener } },
                (k, v) => { v[Session] = listener; return v; }
            );
        }
        public void RemoveTracked(String Session, Guid QueryId)
        {
            IDictionary<String, Listener> listeners;
            if( _subscriptions.TryGetValue(QueryId, out listeners)){
                var newValue = new Dictionary<String, Listener>(listeners);
                newValue.Remove(Session);
                _subscriptions.TryUpdate(QueryId, newValue, listeners);
            }
        }

        public void Publish(Guid QueryId, Int32 Version, Object Payload)
        {
            IDictionary<String, Listener> listeners;
            if (!_subscriptions.TryGetValue(QueryId, out listeners))
                return;

            foreach (var listener in listeners)
            {
                if( listener.Value.Timeout < DateTime.UtcNow )
                {
                    RemoveTracked(listener.Key, QueryId);
                    return;
                }

                _sse.NotifySession(listener.Key, new { Version = Version, Payload = Payload });
            }
        }
    }
}