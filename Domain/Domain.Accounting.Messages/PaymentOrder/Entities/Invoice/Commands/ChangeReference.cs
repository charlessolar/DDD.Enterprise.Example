using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Entities.Invoice.Commands
{
    public class ChangeReference : DemoCommand
    {
        public Guid PaymentOrderId { get; set; }

        public Guid InvoiceId { get; set; }

        public String Reference { get; set; }
    }
}