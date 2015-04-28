using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year/{FiscalYearId}/start", "POST")]
    public class Start : IReturn<Base<Command>>
    {
        public Guid FiscalYearId { get; set; }

        public DateTime Effective { get; set; }
    }
}