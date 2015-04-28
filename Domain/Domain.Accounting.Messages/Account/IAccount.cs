using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account
{
    public interface IAccount : Aggregates.Contracts.IEventSource<Guid>
    {
        Aggregates.SingleValueObject<Boolean> Frozen { get; }
    }
}