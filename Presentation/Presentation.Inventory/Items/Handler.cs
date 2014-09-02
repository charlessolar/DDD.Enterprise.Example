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

            var wrapper = key.FromCache<Item>(_cache);
            if (wrapper == null) return;
            
            wrapper.Payload.Description = e.Description;
            wrapper.UpdateCache(_cache, key);

            var response = wrapper.ToDiff(e);

            foreach (var session in wrapper.Sessions)
                _events.NotifySession(session, "update", response);
        }
    }
}
