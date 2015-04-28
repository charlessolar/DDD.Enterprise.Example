using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store.Services
{
    [Api("Relations")]
    [Route("/relations/store", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
        public Guid? Id { get; set; }

        public String Identity { get; set; }

        public String Name { get; set; }

        public String Address { get; set; }
    }
}