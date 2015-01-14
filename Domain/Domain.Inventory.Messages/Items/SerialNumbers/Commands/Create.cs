using NServiceBus;
using System;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Commands
{
    public class Create : ICommand
    {
        public Guid SerialNumberId { get; set; }

        public String SerialNumber { get; set; }

        public Decimal Quantity { get; set; }

        public DateTime Effective { get; set; }

        public Guid ItemId { get; set; }
    }
}