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
    [Route("/accounting/currency/{CurrencyId}/fraction", "PUT POST")]
    public class ChangeFraction : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }

        public String Fraction { get; set; }
    }
}