using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.FiscalYear
{
    public interface IFiscalYear : Aggregates.Contracts.IEventSource<Guid>
    {
        ValueObjects.Start Started { get; }

        ValueObjects.End Ended { get; }
    }
}