using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order/{PaymentOrderId}/method", "POST")]
    public class ChangeMethod : IReturn<Base<Command>>
    {
        public Guid PaymentOrderId { get; set; }

        public Guid MethodId { get; set; }
    }
}