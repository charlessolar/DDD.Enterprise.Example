using Demo.Infrastructure.Library.SSE;
using Demo.Presentation.Models.Cache.Services;
using ServiceStack;
using System.Threading.Tasks;

namespace Demo.Presentation.Services
{
    public class SSE : Service
    {
        private ISubscriptionManager _manager;

        public SSE(ISubscriptionManager manager)
        {
            _manager = manager;
        }

        public Task Post(Subscribe request)
        {
            _manager.AddTracked(Request.GetPermanentSessionId(), request.Receiver, request.QueryId, request.Timeout);
            return Task.FromResult(true);
        }

        public Task Post(Unsubscribe request)
        {
            _manager.RemoveTracked(Request.GetPermanentSessionId(), request.QueryId);
            return Task.FromResult(true);
        }
    }
}