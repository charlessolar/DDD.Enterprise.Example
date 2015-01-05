using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.SerialNumbers.Events
{
    public interface QuantityTaken : IEvent
    {
        Guid SerialNumberId { get; set; }
        Decimal Quantity { get; set; }
    }
}