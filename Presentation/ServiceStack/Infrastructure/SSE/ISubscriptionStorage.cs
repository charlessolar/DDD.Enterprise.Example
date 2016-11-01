using Demo.Library.SSE;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    public interface ISubscriptionStorage
    {
        Task<IEnumerable<Subscription>> Retreive(Expression<Func<Subscription, bool>> selector);
        Task Store(Subscription subscription);
        Task Remove(Subscription subscription);
        Task Pause(Subscription subscription, bool pause);
        Task Clean();
    }
}
