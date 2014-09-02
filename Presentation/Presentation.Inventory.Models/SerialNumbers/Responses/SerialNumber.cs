using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Models.SerialNumbers.Responses
{
    public class SerialNumber : IHasGuidId
    {
        public Guid Id { get; set; }
        public String Serial { get; set; }
        public Decimal Quantity { get; set; }
        public DateTime Effective { get; set; }
    }
}
