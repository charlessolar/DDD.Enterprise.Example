using Demo.Application.Inventory.SerialNumbers;
using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.SerialNumbers
{
    [Route("/serials/{Id}")]
    public class GetSerialNumber : BasicQuery, IReturn<SerialNumber>
    {
        public Guid Id { get; set; }
    }
}