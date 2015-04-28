using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Entry.Events
{
    public interface Reviewed : IEvent
    {
        Guid JournalId { get; set; }

        Guid EntryId { get; set; }

        Guid EmployeeId { get; set; }
    }
}