using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Identity { get; set; }

        public String Reference { get; set; }

        public STATE State { get; set; }

        public String Method { get; set; }

        public Decimal TotalPayment { get; set; }
    }
}