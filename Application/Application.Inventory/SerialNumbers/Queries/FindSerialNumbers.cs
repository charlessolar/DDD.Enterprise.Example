using Library.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.SerialNumbers.Queries
{
    public class FindSerialNumbers : PagedQuery
    {
        public String Serial { get; set; }
        public DateTime? Effective { get; set; }

        public Guid? ItemId { get; set; }
    }
}