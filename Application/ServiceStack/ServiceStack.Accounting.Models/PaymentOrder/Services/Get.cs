using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order/{PaymentOrderId}", "GET")]
    public class Get : Query<Responses.Get>
    {
        public Guid PaymentOrderId { get; set; }
    }
}