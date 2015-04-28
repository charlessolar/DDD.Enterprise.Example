using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/account", "POST")]
    public class ChangeAccount : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public Guid AccountId { get; set; }
    }
}