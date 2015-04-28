using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Services
{
    [Api("Accounting")]
    [Route("/accounting/payment_order/{PaymentOrderId}/effective", "POST")]
    public class ChangeEffective : IReturn<Base<Command>>
    {
        public Guid PaymentOrderId { get; set; }

        public DateTime Effective { get; set; }
    }
}