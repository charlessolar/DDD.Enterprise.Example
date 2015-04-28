using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public String Symbol { get; set; }

        public Boolean SymbolBefore { get; set; }

        public Decimal RoundingFactor { get; set; }

        public Int32 ComputationalAccuracy { get; set; }

        public String Format { get; set; }

        public String Fraction { get; set; }
    }
}