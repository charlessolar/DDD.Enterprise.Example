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
    [Route("/serials/{Id}")]
    public class GetSerialNumber : BasicQuery, IReturn<SerialNumber>
    {
        public Guid Id { get; set; }
    }
}