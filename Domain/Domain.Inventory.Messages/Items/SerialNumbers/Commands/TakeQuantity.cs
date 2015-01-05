using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Commands
{
    public class TakeQuantity : ICommand
    {
        public Guid ItemId { get; set; }
        public Guid SerialNumberId { get; set; }
        public Decimal Quantity { get; set; }
    }
}