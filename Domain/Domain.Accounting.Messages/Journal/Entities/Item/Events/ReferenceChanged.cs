using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Events
{
    public interface ReferenceChanged : IEvent
    {
        Guid JournalId { get; set; }

        Guid ItemId { get; set; }

        String Reference { get; set; }
    }
}