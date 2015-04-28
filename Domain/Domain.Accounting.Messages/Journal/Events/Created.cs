using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Events
{
    public interface Created : IEvent
    {
        Guid JournalId { get; set; }

        String Code { get; set; }
        String Name { get; set; }

        Guid ResponsibleId { get; set; }

        Boolean CheckDate { get; set; }
        Boolean SkipDraft { get; set; }
    }
}
