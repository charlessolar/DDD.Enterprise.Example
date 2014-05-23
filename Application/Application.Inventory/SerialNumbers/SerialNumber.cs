using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Inventory.SerialNumbers
{
    public interface ISerialNumber
    {
        Guid Id { get; set; }
        String Serial { get; set; }
        Decimal Quantity { get; set; }
        DateTime Effective { get; set; }

        Guid ItemId { get; set; }
    }
}