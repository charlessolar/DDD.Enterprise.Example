
using Demo.Library.Queries;
using Demo.Presentation.Inventory.Models.Items.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Models.Items.Services
{
    [Route("/items", "GET")]
    public class FindItems : PagedQuery, IReturn<Find>
    {

        public String Number { get; set; }
        public String Description { get; set; }
    }

}