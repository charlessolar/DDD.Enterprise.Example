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
    [Route("/accounting/currency/{CurrencyId}/rates", "GET")]
    public class GetRate : PagedQuery<Responses.Rate>
    {
        public Guid CurrencyId { get; set; }
    }
}