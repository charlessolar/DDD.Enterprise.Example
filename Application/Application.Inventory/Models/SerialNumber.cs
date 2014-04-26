using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.Models
{
    public class SerialNumber
    {
        public Guid Id { get; set; }
        public String Serial { get; set; }
        public Decimal Quantity { get; set; }
        public DateTime Effective { get; set; }

        public Guid ItemId { get; set; }
    }
}