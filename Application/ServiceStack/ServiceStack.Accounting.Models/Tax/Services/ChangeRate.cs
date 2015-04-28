using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/rate", "POST")]
    public class ChangeRate : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public Boolean Fixed { get; set; }

        public Decimal Rate { get; set; }
    }
}