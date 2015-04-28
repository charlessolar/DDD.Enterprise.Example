using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/symbol", "PUT POST")]
    public class ChangeSymbol : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }

        public String Symbol { get; set; }
    }
}