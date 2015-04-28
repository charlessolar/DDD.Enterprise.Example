using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/region", "DELETE")]
    public class RemoveRegion : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public Guid RegionId { get; set; }
    }
}