using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Entry
{
    public interface IEntry : Aggregates.Contracts.IEventSource<Guid>
    {
    }
}