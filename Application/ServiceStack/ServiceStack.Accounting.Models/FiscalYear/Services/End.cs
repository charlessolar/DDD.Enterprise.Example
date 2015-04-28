using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year/{FiscalYearId}/end", "POST")]
    public class End : IReturn<Base<Command>>
    {
        public Guid FiscalYearId { get; set; }

        public DateTime Effective { get; set; }
    }
}