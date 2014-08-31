using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Models.Items.Services
{
    [Route("/items/{ItemId}", "POST")]
    public class ChangeDescription : IReturn<IdResponse>
    {
        public Guid ItemId { get; set; }

        public String Description { get; set; }
    }
}
