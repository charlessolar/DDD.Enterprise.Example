using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/select", "GET")]
    public class Select : PagedQuery<Responses.Index>
    {
        public Guid? Id { get; set; }

        public String Term { get; set; }
    }
}