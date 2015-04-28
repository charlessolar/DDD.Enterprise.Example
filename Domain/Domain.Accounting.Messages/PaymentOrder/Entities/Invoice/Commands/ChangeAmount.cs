using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Entities.Invoice.Commands
{
    public class ChangeAmount : DemoCommand
    {
        public Guid PaymentOrderId { get; set; }

        public Guid InvoiceId { get; set; }

        public Decimal Amount { get; set; }
    }
}