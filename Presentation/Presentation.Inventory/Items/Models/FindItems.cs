using Application.Inventory.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Inventory.Items.Models
{
    [Route("/items", "GET")]
    public class FindItems : IReturn<List<Item>>
    {
        public String Number { get; set; }
        public String Description { get; set; }
    }
}