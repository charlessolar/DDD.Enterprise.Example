using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year/{FiscalYearId}/name", "POST")]
    public class ChangeName : IReturn<Base<Command>>
    {
        public Guid FiscalYearId { get; set; }

        public String Name { get; set; }
    }
}