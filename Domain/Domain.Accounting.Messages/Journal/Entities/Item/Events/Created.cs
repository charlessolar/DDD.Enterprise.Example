using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Events
{
    public interface Created : IEvent
    {
        Guid JournalId { get; set; }

        Guid ItemId { get; set; }

        Guid EntryId { get; set; }

        DateTime Effective { get; set; }

        String Reference { get; set; }

        Guid AccountId { get; set; }

        Guid PeriodId { get; set; }

        Decimal Amount { get; set; }
    }
}