using Demo.Domain.Accounting.PaymentOrder.Entities.Invoice.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Entities.Invoice.Handlers
{
    public class Index :
        IHandleMessages<Added>,
        IHandleMessages<AmountChanged>,
        IHandleMessages<DiscountChanged>,
        IHandleMessages<ReferenceChanged>,
        IHandleMessages<Removed>
    {
        private readonly IDocumentStore _store;

        public Index(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Added e)
        {
            using (var session = _store.OpenSession())
            {
                var get = new Responses.Index
                {
                    Id = e.InvoiceId,
                    PaymentOrderId = e.PaymentOrderId,
                };

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(AmountChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Index>(e.InvoiceId);

                get.Amount = e.Amount;

                var paymentOrderGet = session.Load<PaymentOrder.Responses.Get>(e.PaymentOrderId);
                paymentOrderGet.TotalPayment += (e.Amount - get.Amount);
                var paymentOrderIndex = session.Load<PaymentOrder.Responses.Index>(e.PaymentOrderId);
                paymentOrderIndex.TotalPayment += (e.Amount - get.Amount);

                session.SaveChanges();
            }
        }

        public void Handle(DiscountChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Index>(e.InvoiceId);
                get.Discount = e.Discount;

                session.SaveChanges();
            }
        }

        public void Handle(ReferenceChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Index>(e.InvoiceId);
                get.Reference = e.Reference;

                session.SaveChanges();
            }
        }

        public void Handle(Removed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Index>(e.InvoiceId);

                session.Delete(get);
                session.SaveChanges();
            }
        }
    }
}