using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/rates", "PUT POST")]
    public class AddRate : IReturn<Base<Command>>
    {
        public Guid RateId { get; set; }
        public Guid CurrencyId { get; set; }

        public Guid DestinationCurrencyId { get; set; }

        public Decimal Factor { get; set; }

        public DateTime? EffectiveTill { get; set; }
    }
}