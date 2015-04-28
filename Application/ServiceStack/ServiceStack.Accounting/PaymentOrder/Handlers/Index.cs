using Demo.Domain.Accounting.PaymentOrder.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Invoice = Demo.Domain.Accounting.PaymentOrder.Entities.Invoice;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Handlers
{
    public class Index :
        IHandleMessages<Confirmed>,
        IHandleMessages<Discarded>,
        IHandleMessages<Dispursed>,
        IHandleMessages<MethodChanged>,
        IHandleMessages<ReferenceChanged>,
        IHandleMessages<Started>,
        IHandleMessages<Invoice.Events.AmountChanged>
    {
        private readonly IDocumentStore _store;

        public Index(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Confirmed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.PaymentOrderId);
                index.State = Responses.STATE.Confirmed;

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Discarded e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.PaymentOrderId);
                index.State = Responses.STATE.Discarded;

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Dispursed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.PaymentOrderId);
                index.State = Responses.STATE.Dispursed;

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Invoice.Events.AmountChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.PaymentOrderId);
                var invoice = session.Load<Entities.Invoice.Responses.Index>(e.InvoiceId);

                index.TotalPayment += (e.Amount - invoice.Amount);

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(MethodChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.PaymentOrderId);
                var method = session.Load<Configuration.PaymentMethod.Responses.Index>(e.MethodId);
                index.Method = method.Name;

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(ReferenceChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.PaymentOrderId);
                index.Reference = e.Reference;

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Started e)
        {
            using (var session = _store.OpenSession())
            {
                var index = new Responses.Index
                {
                    Id = e.PaymentOrderId,
                    Identity = e.Identity
                };

                session.Store(index);
                session.SaveChanges();
            }
        }
    }
}