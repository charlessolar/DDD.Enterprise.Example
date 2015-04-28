using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Entities.Period.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year/{FiscalYearId}/period/{PeriodId}/start", "POST")]
    public class End : IReturn<Base<Command>>
    {
        public Guid FiscalYearId { get; set; }

        public Guid PeriodId { get; set; }

        public DateTime Effective { get; set; }
    }
}