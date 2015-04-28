using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal
{
    public interface IJournal : Aggregates.Contracts.IEventSource<Guid>
    {
        ValueObjects.SkipDraft SkipDraft { get; }

        ValueObjects.CheckDate CheckDate { get; }
    }
}