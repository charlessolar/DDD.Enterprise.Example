using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}", "DELETE")]
    public class Destroy : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }
    }
}