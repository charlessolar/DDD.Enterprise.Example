using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year/{FiscalYearId}", "DELETE")]
    public class Destroy : IReturn<Base<Command>>
    {
        public Guid FiscalYearId { get; set; }
    }
}