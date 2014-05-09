using Demo.Library.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Inventory.Items.Queries
{
    public class FindItems : PagedQuery
    {
        public String Number { get; set; }
        public String Description { get; set; }
    }
}