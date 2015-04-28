using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.TaxType.Services
{
    [Api("Configuration")]
    [Route("/configuration/tax-type", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
    }
}