using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Entities.Invoice.Events
{
    public interface PaymentCreated : IEvent
    {
        Guid PaymentOrderId { get; set; }

        Guid InvoiceId { get; set; }

        Guid SupplierInvoiceId { get; set; }

        Guid SupplierPaymentId { get; set; }

        DateTime Effective { get; set; }
    }
}