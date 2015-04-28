using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Services
{
    [Api("Accounting")]
    [Route("/accounting/tax/{TaxId}/deactivate", "POST")]
    public class Deactivate : IReturn<Base<Command>>
    {
        public Guid TaxId { get; set; }

        public Guid EmployeeId { get; set; }
    }
}