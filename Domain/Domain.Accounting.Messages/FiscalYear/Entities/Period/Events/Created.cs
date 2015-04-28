using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Entities.Period.Events
{
    public interface Created : IEvent
    {
        Guid FiscalYearId { get; set; }

        Guid PeriodId { get; set; }

        String Code { get; set; }

        String Name { get; set; }
    }
}