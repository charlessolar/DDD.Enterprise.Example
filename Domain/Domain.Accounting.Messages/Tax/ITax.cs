using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax
{
    public interface ITax : Aggregates.Contracts.IEventSource<Guid>
    {
        Aggregates.SingleValueObject<Boolean> Activated { get; }

        ValueObjects.Rate Rate { get; }
    }
}