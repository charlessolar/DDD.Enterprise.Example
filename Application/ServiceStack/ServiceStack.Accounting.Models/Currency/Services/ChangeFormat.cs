using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/format", "PUT POST")]
    public class ChangeFormat : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }

        public String Format { get; set; }
    }
}