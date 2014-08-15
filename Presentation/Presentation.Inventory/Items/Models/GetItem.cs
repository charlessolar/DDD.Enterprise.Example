using Demo.Application.Inventory.Items;
using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items.Models
{
    [Route("/items/{Id}")]
    public class GetItem : BasicQuery, IReturn<Item>
    {
        public Guid Id { get; set; }
    }
}