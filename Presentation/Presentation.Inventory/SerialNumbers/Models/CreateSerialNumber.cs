using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Inventory.SerialNumbers.Models
{
    [Route("/serials", "POST")]
    public class CreateSerialNumber
    {
        public String SerialNumber { get; set; }
        public Decimal Quantity { get; set; }
        public DateTime Effective { get; set; }
        public Guid ItemId { get; set; }
    }
}