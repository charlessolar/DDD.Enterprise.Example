using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.SerialNumbers
{
    [Route("/serials/{Id}/delete", "DELETE")]
    public class DeleteSerialNumber
    {
        public Guid Id { get; set; }
    }
}