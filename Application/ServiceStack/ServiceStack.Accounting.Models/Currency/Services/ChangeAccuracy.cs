using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/accuracy", "PUT POST")]
    public class ChangeAccuracy : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }

        public Int32 Accuracy { get; set; }
    }
}