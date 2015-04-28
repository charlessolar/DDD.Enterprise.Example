using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Events
{
    public interface Started : IEvent
    {
        Guid FiscalYearId { get; set; }

        DateTime Effective { get; set; }
    }
}