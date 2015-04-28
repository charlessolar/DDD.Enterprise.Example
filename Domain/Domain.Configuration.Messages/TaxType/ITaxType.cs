using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.TaxType
{
    public interface ITaxType : Aggregates.Contracts.IEventSource<Guid>
    {
        ValueObjects.Name Name { get; }
    }
}