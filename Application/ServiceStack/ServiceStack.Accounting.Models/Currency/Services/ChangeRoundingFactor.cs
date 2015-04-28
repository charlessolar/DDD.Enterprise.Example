using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/rounding", "PUT POST")]
    public class ChangeRoundingFactor : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }

        public Decimal RoundingFactor { get; set; }
    }
}