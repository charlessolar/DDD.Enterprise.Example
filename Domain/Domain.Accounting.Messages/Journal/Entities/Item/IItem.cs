using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item
{
    public interface IItem : Aggregates.Contracts.IEventSource<Guid>
    {
        ValueObjects.Amount Amount { get; }

        ValueObjects.ReconciledAmount Reconciled { get; }
    }
}