using Demo.Library.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Models.Items.Responses
{
    public class Find : IIsList<Item>
    {
        public IEnumerable<Item> Results { get; set; }
    }
}
