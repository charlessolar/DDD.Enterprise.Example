using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Events
{
    public interface MethodChanged : IEvent
    {
        Guid PaymentOrderId { get; set; }

        Guid MethodId { get; set; }
    }
}