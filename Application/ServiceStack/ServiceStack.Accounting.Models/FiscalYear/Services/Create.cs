using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid FiscalYearId { get; set; }

        public String Name { get; set; }

        public String Code { get; set; }
    }
}