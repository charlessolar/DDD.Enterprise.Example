using Demo.Domain.Inventory.Items.Events;
using Demo.Infrastructure.Library.SSE;
using Demo.Library.ServiceStack.Responses;
using NServiceBus;
using ServiceStack;
using ServiceStack.Caching;
using System;

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
            var key = UrnId.Create("Item", e.ItemId.ToString());
            var sse = new Event<Created>
            {
                Domain = "Inventory.Item",
                EventName = "Created",
                Urn = key,
                Updated = DateTime.UtcNow,
                Payload = e
            };

        }

        public void Handle(DescriptionChanged e)
        {
            var key = UrnId.Create("Item", e.ItemId.ToString());
            var sse = new Event<DescriptionChanged>
            {
                Domain = "Inventory.Item",
                EventName = "DescriptionChanged",
                Urn = key,
                Updated = DateTime.UtcNow,
                Payload = e
            };
        }
    }
}