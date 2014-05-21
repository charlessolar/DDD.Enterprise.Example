using Demo.Application.Inventory.Items;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items.Models
{
    [Route("/items/{Id}")]
    public class GetItem
    {
        public Guid Id { get; set; }
    }
}