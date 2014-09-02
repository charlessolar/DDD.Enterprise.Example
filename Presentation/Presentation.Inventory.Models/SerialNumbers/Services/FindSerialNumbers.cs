using Demo.Application.Inventory.SerialNumbers;
using Demo.Library.Queries;
using Demo.Presentation.Inventory.Models.SerialNumbers.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Models.SerialNumbers.Services
{
    [Route("/serials", "GET")]
    public class FindSerialNumbers : PagedQuery, IReturn<Find>
    {

        public String Serial { get; set; }
        public DateTime? Effective { get; set; }

        public Guid? ItemId { get; set; }
    }
}