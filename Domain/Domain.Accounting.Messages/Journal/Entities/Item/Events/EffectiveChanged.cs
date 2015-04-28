using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Events
{
    public interface EffectiveChanged : IEvent
    {
        Guid JournalId { get; set; }

        Guid ItemId { get; set; }

        DateTime Effective { get; set; }
    }
}