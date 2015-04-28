using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Events
{
    public interface ReferenceChanged : IEvent
    {
        Guid PaymentOrderId { get; set; }

        String Reference { get; set; }
    }
}