using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod
{
    public interface IPaymentMethod : Aggregates.Contracts.IEventSource<Guid>
    {
    }
}
