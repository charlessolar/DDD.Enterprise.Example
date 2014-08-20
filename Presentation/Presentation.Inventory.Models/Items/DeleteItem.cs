using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items
{
    [Route("/items/{Id}/delete", "DELETE")]
    public class DeleteItem
    {
        public Guid Id { get; set; }
    }
}