using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee
{
    public interface IEmployee : Aggregates.Contracts.IEventSource<Guid>
    {
        Aggregates.SingleValueObject<String> Identity { get; }

        Aggregates.SingleValueObject<Boolean> Employed { get; }
    }
}