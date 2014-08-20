using Demo.Library.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.SerialNumbers.Identities
{
    public class SerialNumber : Identity<SerialNumber>
    {
        public String Serial { get; set; }
        public Decimal Quantity { get; set; }
        public DateTime Effective { get; set; }
        public Guid ItemId { get; set; }
    }
}