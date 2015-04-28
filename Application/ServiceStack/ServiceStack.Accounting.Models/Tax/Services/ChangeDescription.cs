using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/description", "POST")]
    public class ChangeDescription : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public String Description { get; set; }
    }
}