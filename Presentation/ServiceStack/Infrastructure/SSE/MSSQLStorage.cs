using Demo.Library.SSE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Linq.Expressions;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public class MssqlStorage : ISubscriptionStorage
    {
        private readonly IDbConnectionFactory _sql;

        public MssqlStorage(IDbConnectionFactory sql)
        {
            _sql = sql;
        }

        public async Task Pause(Subscription subscription, bool pause)
        {
            using (var session = _sql.Open())
            {
                var existing = await session.SingleByIdAsync<Subscription>(subscription.Id).ConfigureAwait(false);
                if (existing == null) return;

                existing.Paused = pause;

                await session.UpdateOnlyAsync(existing, onlyFields: x => x.Paused, @where: x => x.SubscriptionId == subscription.SubscriptionId).ConfigureAwait(false);

            }
        }

        public async Task Remove(Subscription subscription)
        {
            using (var session = _sql.Open())
            {
                await session.DeleteByIdAsync<Subscription>(subscription.SubscriptionId).ConfigureAwait(false);
            }
        }

        public async Task<IEnumerable<Subscription>> Retreive(Expression<Func<Subscription, bool>> selector)
        {
            using (var session = _sql.Open())
            {
                // var parameter = Expression.Parameter(typeof(ISubscription), "x");

                //var expression = Expression.Lambda<Func<ISubscription, bool>>(Expression.Call(selector.Method), parameter);
                var fromSql = session.From<Subscription>().Where(selector);

                return await session.SelectAsync(fromSql).ConfigureAwait(false);
            }
        }

        public async Task Store(Subscription subscription)
        {
            using (var session = _sql.Open())
            {
                var existing = await session.SingleByIdAsync<Subscription>(subscription.Id).ConfigureAwait(false);
                if (existing != null)
                {
                    existing.Expires = subscription.Expires;
                    await session.UpdateOnlyAsync<Subscription>(existing, onlyFields: x => x.Expires, @where: x => x.SubscriptionId == subscription.SubscriptionId).ConfigureAwait(false);
                    return;
                }

                await session.InsertAsync<Subscription>(subscription).ConfigureAwait(false);

            }
        }
        public async Task Clean()
        {
            using (var session = _sql.Open())
            {
                await session.DeleteAsync<Subscription>(x => x.Expires < DateTime.UtcNow).ConfigureAwait(false);
            }
        }
    }
}
