using Demo.Domain.Inventory.Items.Events;
using Demo.Library.Extensions;
using NServiceBus;
using Demo.Presentation.Inventory.Models.Items.Responses;
using ServiceStack;
using ServiceStack.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Responses;
using Demo.Infrastructure.Library.SSE;
using Demo.Library.ServiceStack.Responses;

namespace Demo.Presentation.Inventory.Items
{
    public class Handler : IHandleMessages<Created>, IHandleMessages<DescriptionChanged>
    {
        private IServerEvents _events { get; set; }
        private ICacheClient _cache { get; set; }
        private ISubscriptionManager _manager { get; set; }

        public Handler(IServerEvents events, ICacheClient cache, ISubscriptionManager manager)
        {
            _events = events;
            _cache = cache;
            _manager = manager;
        }

        public void Handle(Created e)
        {
            var subs = _manager.GetSubscriptions("Inventory.Item");

            var key = UrnId.Create<Item>(e.ItemId);
            var sse = new Event<Created>
            {
                Domain = "Inventory.Item",
                EventName = "Created",
                Urn = key,
                Updated = DateTime.UtcNow,
                Payload = e
            };

            foreach (var session in subs)
                _events.NotifySession(session, "events", sse);

        }
        public void Handle(DescriptionChanged e)
        {
            var subs = _manager.GetSubscriptions("Inventory.Item");

            var key = UrnId.Create<Item>(e.ItemId);
            var sse = new Event<DescriptionChanged>
            {
                Domain = "Inventory.Item",
                EventName = "DescriptionChanged",
                Urn = key,
                Updated = DateTime.UtcNow,
                Payload = e
            };            

            foreach (var session in subs)
                _events.NotifySession(session, "events", sse);
        }
    }
}
