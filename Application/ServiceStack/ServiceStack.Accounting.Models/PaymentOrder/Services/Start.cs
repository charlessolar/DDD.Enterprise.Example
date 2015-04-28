using Demo.Library.Responses;

using ServiceStack;

using System;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order", "POST")]
    public class Start : IReturn<Base<Command>>
    {
        public Guid PaymentOrderId { get; set; }

        public String Identity { get; set; }
    }
}