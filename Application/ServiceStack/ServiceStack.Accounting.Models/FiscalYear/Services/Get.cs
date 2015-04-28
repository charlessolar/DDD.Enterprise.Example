using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year/{FiscalYearId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid FiscalYearId { get; set; }
    }
}