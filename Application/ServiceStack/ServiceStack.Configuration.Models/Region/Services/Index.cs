using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Services
{
    [Api("Configuration")]
    [Route("/configuration/region", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
    }
}