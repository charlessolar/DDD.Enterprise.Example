using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Entities.Period.Services
{
    [Api("Accounting")]
    [Route("/accounting/fiscal_year/{FiscalYearId}/period/{PeriodId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid FiscalYearId { get; set; }

        public Guid PeriodId { get; set; }
    }
}