using Demo.Application.Inventory.Items;
using Demo.Application.Inventory.Items.Messages;
using NServiceBus;
using ServiceStack.Caching;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items
{
    public class EventHandler : IHandleMessages<ItemsRetreived>
    {
        public void Handle(ItemsRetreived m)
        {
        }
    }
}