using Demo.Infrastructure.Library.SSE;
using Demo.Library.Responses;
using Demo.Presentation.Models.Cache.Services;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Demo.Presentation.Services
{
    public class SSE : Service
    {
        private ISubscriptionManager _manager;

        public SSE( ISubscriptionManager manager )
        {
            _manager = manager;
        }

        public Task<Base> Post( Subscribe request )
        {
            _manager.Subscribe(Request.GetPermanentSessionId(), request.Domain);
            return Task.FromResult(new Base { Status = "success" });
        }

        public Task<Base> Post(Unsubscribe request)
        {
            _manager.Unsubscribe(Request.GetPermanentSessionId(), request.Domain);
            return Task.FromResult(new Base { Status = "success" });
        }
    }
}