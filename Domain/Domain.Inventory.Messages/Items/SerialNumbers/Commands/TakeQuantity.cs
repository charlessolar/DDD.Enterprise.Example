using NServiceBus;
using System;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Commands
{
    public class TakeQuantity : ICommand
    {
        public Guid ItemId { get; set; }

        public Guid SerialNumberId { get; set; }

        public Decimal Quantity { get; set; }
    }
}