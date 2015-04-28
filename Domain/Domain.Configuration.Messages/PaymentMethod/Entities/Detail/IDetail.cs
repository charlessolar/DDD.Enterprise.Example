using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod.Entities.Detail
{
    public interface IDetail : Aggregates.Contracts.IEventSource<Guid>
    {
    }
}