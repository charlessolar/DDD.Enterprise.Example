using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/activate", "PUT POST")]
    public class Activate : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }
    }
}