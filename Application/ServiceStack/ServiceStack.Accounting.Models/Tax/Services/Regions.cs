using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/regions", "GET")]
    public class Regions : PagedQuery<Responses.Region>
    {
        public Guid TaxId { get; set; }
    }
}