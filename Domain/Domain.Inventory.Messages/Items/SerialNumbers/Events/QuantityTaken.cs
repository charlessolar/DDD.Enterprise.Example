using NServiceBus;
using System;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Events
{
    public interface QuantityTaken : IEvent
    {
        Guid SerialNumberId { get; set; }

        Decimal Quantity { get; set; }
    }
}