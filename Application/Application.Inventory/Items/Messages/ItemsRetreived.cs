using Library.Queries;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.Items.Messages
{
    public class ItemsRetreived : IMessage
    {
        public IEnumerable<Item> Items { get; set; }
    }
}