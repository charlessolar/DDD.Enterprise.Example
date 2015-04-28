using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Entities.Invoice.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public Guid PaymentOrderId { get; set; }

        public Guid InvoiceId { get; set; }

        public String InvoiceIdentity { get; set; }

        public Guid SupplierId { get; set; }

        public String Supplier { get; set; }

        public DateTime? Paid { get; set; }

        public Guid? PaymentId { get; set; }

        public String PaymentIdentity { get; set; }

        public Decimal? Discount { get; set; }

        public String Reference { get; set; }

        public Decimal Amount { get; set; }
    }
}