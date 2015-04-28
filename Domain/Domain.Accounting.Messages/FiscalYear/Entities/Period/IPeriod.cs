using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear.Entities.Period
{
    public interface IPeriod : Aggregates.Contracts.IEventSource<Guid>
    {
        ValueObjects.Start Started { get; }

        ValueObjects.End Ended { get; }
    }
}