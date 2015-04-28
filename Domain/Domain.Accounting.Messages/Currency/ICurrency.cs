using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency
{
    public interface ICurrency : Aggregates.Contracts.IEventSource<Guid>
    {
        Aggregates.SingleValueObject<Boolean> Activated { get; }
    }
}