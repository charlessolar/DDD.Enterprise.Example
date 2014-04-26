using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory.SerialNumbers.Events
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