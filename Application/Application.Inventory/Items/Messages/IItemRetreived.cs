using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.Items.Messages
{
    public class ItemRetreived : IMessage
    {
        public Item Item { get; set; }
    }
}