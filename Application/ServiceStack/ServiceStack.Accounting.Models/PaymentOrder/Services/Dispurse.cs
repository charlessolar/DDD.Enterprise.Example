using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order/{PaymentOrderId}/dispurse", "POST")]
    public class Dispurse : IReturn<Base<Command>>
    {
        public Guid PaymentOrderId { get; set; }
    }
}