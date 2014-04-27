using Application.Inventory.Items;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Inventory.Items.Models
{
    [Route("/items/{Id}")]
    public class GetItem : IReturn<Item>
    {
        public Guid Id { get; set; }
    }
}