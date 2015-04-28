using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder.Entities.Invoice
{
    public class Invoice : Aggregates.Entity<Guid>, IInvoice
    {
        private Invoice()
        {
        }

        public void Add()
        {
            Apply<Events.Added>(e =>
            {
                e.PaymentOrderId = AggregateId;
                e.InvoiceId = Id;
            });
        }

        public void ChangeAmount(Decimal Amount)
        {
            Apply<Events.AmountChanged>(e =>
            {
                e.PaymentOrderId = AggregateId;
                e.InvoiceId = Id;
                e.Amount = Amount;
            });
        }

        public void ChangeDiscount(Decimal? Discount)
        {
            Apply<Events.DiscountChanged>(e =>
            {
                e.PaymentOrderId = AggregateId;
                e.InvoiceId = Id;
                e.Discount = Discount;
            });
        }

        public void ChangeReference(String Reference)
        {
            Apply<Events.ReferenceChanged>(e =>
            {
                e.PaymentOrderId = AggregateId;
                e.InvoiceId = Id;
                e.Reference = Reference;
            });
        }

        public void Remove()
        {
            Apply<Events.Removed>(e =>
            {
                e.PaymentOrderId = AggregateId;
                e.InvoiceId = Id;
            });
        }
    }
}