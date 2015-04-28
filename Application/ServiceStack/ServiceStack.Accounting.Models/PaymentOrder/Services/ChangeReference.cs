using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order/{PaymentOrderId}/reference", "POST")]
    public class ChangeReference : IReturn<Base<Command>>
    {
        public Guid PaymentOrderId { get; set; }

        public String Reference { get; set; }
    }
}