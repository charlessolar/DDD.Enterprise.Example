using Demo.Library.Queries;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Services
{
    [Api("Accounting")]
    [Route("/accounting/currency/{CurrencyId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid CurrencyId { get; set; }
    }
}