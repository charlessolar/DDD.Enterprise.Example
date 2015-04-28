using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Responses
{
    public class Get : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Identity { get; set; }

        public STATE State { get; set; }

        public DateTime? Effective { get; set; }

        public String Method { get; set; }

        public String Reference { get; set; }

        public Decimal TotalPayment { get; set; }

        public ICollection<Guid> Items { get; set; }
    }
}