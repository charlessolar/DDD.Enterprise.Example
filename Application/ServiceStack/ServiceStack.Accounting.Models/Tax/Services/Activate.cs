using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/activate", "POST")]
    public class Activate : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public Guid EmployeeId { get; set; }
    }
}