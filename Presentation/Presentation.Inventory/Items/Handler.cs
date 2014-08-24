using Demo.Domain.Inventory.Items.Events;
using NServiceBus;
using ServiceStack;
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

        public Handler(IServerEvents events)
        {
            _events = events;
        }

        public void Handle(Created e)
        {
            _events.NotifyChannel("events", "Item.Created", e);
        }
        public void Handle(DescriptionChanged e)
        {
            _events.NotifyChannel("events", "Item.DescriptionChanged", e);
        }
    }
}
