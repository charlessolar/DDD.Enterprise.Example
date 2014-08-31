using Demo.Domain.Inventory.Items.Events;
using Demo.Library.Cache;
using NServiceBus;
using Demo.Presentation.Inventory.Models.Items.Responses;
using ServiceStack;
using ServiceStack.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items
{
    public class Handler : IHandleMessages<Created>, IHandleMessages<DescriptionChanged>
    {
        private IServerEvents _events { get; set; }
        private ICacheClient _cache { get; set; }

        public Handler(IServerEvents events, ICacheClient cache)
        {
            _events = events;
            _cache = cache;
        }

        public void Handle(Created e)
        {

        }
        public void Handle(DescriptionChanged e)
        {
            var key =  UrnId.Create<Item>(e.ItemId);

            var wrapper = key.FromCache(_cache);
            if (wrapper == null) return;

            var item = wrapper.Payload as Item;

            item.Description = e.Description;

            item.UpdateCache(_cache);

            foreach (var session in wrapper.Sessions)
                _events.NotifySession(session, key, item);
        }
    }
}
