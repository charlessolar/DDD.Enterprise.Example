using NServiceBus;
using System;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Events
{
    public interface Created : IEvent
    {
        Guid SerialNumberId { get; set; }

        String SerialNumber { get; set; }

        Decimal Quantity { get; set; }

        DateTime Effective { get; set; }

        Guid ItemId { get; set; }
    }
}