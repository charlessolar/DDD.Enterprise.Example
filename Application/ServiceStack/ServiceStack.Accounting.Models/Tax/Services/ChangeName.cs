using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/name", "POST")]
    public class ChangeName : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public String Name { get; set; }
    }
}