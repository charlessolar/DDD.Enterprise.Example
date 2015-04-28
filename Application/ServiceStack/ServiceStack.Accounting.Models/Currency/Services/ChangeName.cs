using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/name", "PUT POST")]
    public class ChangeName : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }

        public String Name { get; set; }
    }
}