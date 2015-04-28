using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/symbol_before", "PUT POST")]
    public class SymbolBefore : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }

        public Boolean Before { get; set; }
    }
}