using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Infrastructure.Library.SSE
{
    public class MemorySubscriptionManager : ISubscriptionManager
    {
        private Dictionary<String, ISet<String>> _subscriptions;

        public MemorySubscriptionManager()
        {
            _subscriptions = new Dictionary<String, ISet<String>>();
        }

        public Boolean IsSubscribed(String Session, String Domain)
        {
            ISet<String> sessions;
            if (!_subscriptions.TryGetValue(Domain, out sessions))
                return false;

            if (!sessions.Contains(Domain))
                return false;

            return true;
        }

        public ISet<String> GetSubscriptions(String Domain)
        {
            ISet<String> sessions;
            if (!_subscriptions.TryGetValue(Domain, out sessions))
                return new HashSet<String>();

            return sessions;
        }

        public void Subscribe(String Session, String Domain)
        {
            if (String.IsNullOrEmpty(Session)) return;
            if (String.IsNullOrEmpty(Domain)) return;

            ISet<String> sessions;
            if (_subscriptions.TryGetValue(Domain, out sessions))
            {
                sessions.Add(Session);
                return;
            }

            sessions = new HashSet<String>();
            sessions.Add(Session);

            _subscriptions[Domain] = sessions;
        }

        public void Unsubscribe(String Session, String Domain)
        {
            if (String.IsNullOrEmpty(Session)) return;
            if (String.IsNullOrEmpty(Domain)) return;

            ISet<String> sessions;
            if (!_subscriptions.TryGetValue(Domain, out sessions))
                return;

            sessions.Remove(Session);
        }
    }
}