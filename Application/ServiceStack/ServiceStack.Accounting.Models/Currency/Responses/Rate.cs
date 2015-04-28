using Demo.Library.Responses;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Responses
{
    public class Rate : IResponse
    {
        public Guid Id { get; set; }
        public Guid CurrencyId { get; set; }

        public Guid DestinationCurrencyId { get; set; }

        public String Exchange { get; set; }

        public Decimal Factor { get; set; }
        
        public DateTime? EffectiveTill { get; set; }
    }
}