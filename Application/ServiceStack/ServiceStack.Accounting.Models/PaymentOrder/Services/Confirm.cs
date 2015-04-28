using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order/{PaymentOrderId}/confirm", "POST")]
    public class Confirm : IReturn<Base<Command>>
    {
        public Guid PaymentOrderId { get; set; }
    }
}