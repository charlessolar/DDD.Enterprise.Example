using Application.Inventory.Items.Messages;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Inventory.Items
{
    public class EventHandler : IHandleMessages<ItemRetreived>
    {
        public void Handle(ItemRetreived m)
        {
            Console.WriteLine("Received item!");
        }
    }
}