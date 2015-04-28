using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Guid TaxTypeId { get; set; }
    }
}