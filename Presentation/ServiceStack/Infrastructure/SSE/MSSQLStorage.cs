using Newtonsoft.Json;
using Demo.Presentation.ServiceStack.Infrastructure.Extensions;
using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using Demo.Library.SSE;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Text;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Linq.Expressions;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public class MSSQLStorage : ISubscriptionStorage
    {
        private readonly IDbConnectionFactory _sql;

        public MSSQLStorage(IDbConnectionFactory sql)
        {
            _sql = sql;
        }

        public async Task Pause(Subscription subscription, bool pause)
        {
            using (var session = _sql.Open())
            {
                var existing = await session.SingleByIdAsync<Subscription>(subscription.Id);
                if (existing == null) return;

                existing.Paused = pause;

                await session.UpdateOnlyAsync(existing, onlyFields: x => x.Paused, where: x => x.SubscriptionId == subscription.SubscriptionId);

            }
        }

        public async Task Remove(Subscription subscription)
        {
            using (var session = _sql.Open())
            {
                await session.DeleteByIdAsync<Subscription>(subscription.SubscriptionId);
            }
        }

        public async Task<IEnumerable<Subscription>> Retreive(Expression<Func<Subscription, bool>> selector)
        {
            using (var session = _sql.Open())
            {
                // var parameter = Expression.Parameter(typeof(ISubscription), "x");

                //var expression = Expression.Lambda<Func<ISubscription, bool>>(Expression.Call(selector.Method), parameter);
                var fromSql = session.From<Subscription>().Where(selector);

                return await session.SelectAsync(fromSql);
            }
        }

        public async Task Store(Subscription subscription)
        {
            using (var session = _sql.Open())
            {
                var existing = await session.SingleByIdAsync<Subscription>(subscription.Id);
                if (existing != null)
                {
                    existing.Expires = subscription.Expires;
                    await session.UpdateOnlyAsync<Subscription>(existing, onlyFields: x => x.Expires, where: x => x.SubscriptionId == subscription.SubscriptionId);
                    return;
                }

                await session.InsertAsync<Subscription>(subscription);

            }
        }
        public async Task Clean()
        {
            using (var session = _sql.Open())
            {
                await session.DeleteAsync<Subscription>(x => x.Expires < DateTime.UtcNow);
            }
        }
    }
}
