using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Entities.Invoice.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order/{PaymentOrderId}/invoice/{InvoiceId}/amount", "POST")]
    public class ChangeAmount : IReturn<Base<Command>>
    {
        public Guid PaymentOrderId { get; set; }

        public Guid InvoiceId { get; set; }

        public Decimal Amount { get; set; }
    }
}