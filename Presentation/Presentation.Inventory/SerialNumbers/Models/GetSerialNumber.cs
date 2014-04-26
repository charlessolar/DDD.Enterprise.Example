using Application.Inventory.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Inventory.SerialNumbers.Models
{
    [Route("/serials/{Id}")]
    public class GetSerialNumber : IReturn<SerialNumber>
    {
        public Guid Id { get; set; }
    }
}