using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Entities.Invoice.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order/{PaymentOrderId}/invoice", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
        public Guid PaymentOrderId { get; set; }
    }
}