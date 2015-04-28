using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}/deactivate", "PUT POST")]
    public class Deactivate : IReturn<Base<Command>>
    {
        public Guid CurrencyId { get; set; }
    }
}