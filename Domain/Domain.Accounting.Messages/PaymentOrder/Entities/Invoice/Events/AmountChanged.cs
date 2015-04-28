using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Entities.Invoice.Events
{
    public interface AmountChanged : IEvent
    {
        Guid PaymentOrderId { get; set; }

        Guid InvoiceId { get; set; }

        Decimal Amount { get; set; }
    }
}