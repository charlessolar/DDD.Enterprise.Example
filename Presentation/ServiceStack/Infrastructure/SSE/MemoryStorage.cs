using Demo.Library.SSE;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public class MemoryStorage : ISubscriptionStorage
    {
        private readonly IList<Subscription> _subscriptions = new List<Subscription>();


        public Task<IEnumerable<Subscription>> Retreive(Expression<Func<Subscription, bool>> selector)
        {
            lock (_subscriptions)
            {
                var results = _subscriptions.Where(x => selector.Compile().Invoke(x)).ToList().AsEnumerable();
                return Task.FromResult(results);
            }
        }

        public Task Pause(Subscription subscription, Boolean paused)
        {
            lock (_subscriptions)
            {
                var existing = this._subscriptions.SingleOrDefault(x => x.SubscriptionId == subscription.SubscriptionId && x.Document == subscription.Document && x.DocumentId == subscription.DocumentId);
                if (existing == null) return Task.FromResult(0);

                existing.Paused = paused;
            }
            return Task.FromResult(0);
        }

        public Task Remove(Subscription subscription)
        {
            lock (_subscriptions)
            {
                this._subscriptions.Remove(subscription);
            }
            return Task.FromResult(0);
        }

        public Task Store(Subscription subscription)
        {
            lock (_subscriptions)
            {
                var existing = this._subscriptions.SingleOrDefault(x => x.SubscriptionId == subscription.SubscriptionId && x.Document == subscription.Document && x.DocumentId == subscription.DocumentId);
                if (existing != null)
                {
                    existing.Expires = subscription.Expires;
                    return Task.FromResult(0);
                }

                this._subscriptions.Add(subscription);
            }
            return Task.FromResult(0);
        }
        public Task Clean()
        {
            lock (_subscriptions)
            {
                var existing = this._subscriptions.Where(x => x.Expires < DateTime.UtcNow).ToList();
                existing.ForEach(x => this._subscriptions.Remove(x));
            }
            return Task.FromResult(0);
        }
    }
}
