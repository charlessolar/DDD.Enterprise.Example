using Demo.Application.Inventory.Items;
using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items
{
    [Route("/items", "GET")]
    public class FindItems : PagedQuery, IReturn<IEnumerable<Item>>
    {

        public String Number { get; set; }
        public String Description { get; set; }
    }
}