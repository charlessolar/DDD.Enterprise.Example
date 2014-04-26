using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory.SerialNumbers.Commands
{
    public class TakeQuantity : ICommand
    {
        public Guid SerialNumberId { get; set; }
        public Decimal Quantity { get; set; }
    }
}