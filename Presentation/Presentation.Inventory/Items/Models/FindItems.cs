using Demo.Application.Inventory.Items;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items.Models
{
    [Route("/items", "GET")]
    public class FindItems
    {
        public Int32 Page { get; set; }
        public Int32 PageSize { get; set; }

        public String Number { get; set; }
        public String Description { get; set; }
    }
}