using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Entities.Period.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year/{FiscalYearId}/period/{PeriodId}", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid FiscalYearId { get; set; }

        public Guid PeriodId { get; set; }

        public String Name { get; set; }

        public String Code { get; set; }
    }
}