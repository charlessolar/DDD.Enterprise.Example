using Demo.Presentation.ServiceStack.Infrastructure.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Utils.Services
{
    [Api("Utilities")]
    [Route("/util/clients", "GET")]
    public class Clients : Queries_Paged<Responses.Client>
    {
        public String Username { get; set; }
    }
}
