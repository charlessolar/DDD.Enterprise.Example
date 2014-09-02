using Demo.Library.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Models.SerialNumbers.Responses
{
    public class Find : IIsList<SerialNumber>
    {
        public IEnumerable<SerialNumber> Results { get; set; }
    }
}
