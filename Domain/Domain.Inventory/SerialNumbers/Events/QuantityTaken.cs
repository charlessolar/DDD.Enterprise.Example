using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Inventory.SerialNumbers.Events
{
    public interface QuantityTaken
    {
        Guid SerialNumberId { get; set; }
        Decimal Quantity { get; set; }
    }
}