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
    public class Get :
        IHandleMessages<Confirmed>,
        IHandleMessages<Discarded>,
        IHandleMessages<Dispursed>,
        IHandleMessages<EffectiveChanged>,
        IHandleMessages<MethodChanged>,
        IHandleMessages<ReferenceChanged>,
        IHandleMessages<Started>,
        IHandleMessages<Invoice.Events.AmountChanged>
    {
        private readonly IDocumentStore _store;

        public Get(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Confirmed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.PaymentOrderId);
                get.State = Responses.STATE.Confirmed;

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Discarded e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.PaymentOrderId);
                get.State = Responses.STATE.Discarded;

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Dispursed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.PaymentOrderId);
                get.State = Responses.STATE.Dispursed;

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(EffectiveChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.PaymentOrderId);
                get.Effective = e.Effective;

                session.Store(get);
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
                var get = session.Load<Responses.Get>(e.PaymentOrderId);
                var method = session.Load<Configuration.PaymentMethod.Responses.Index>(e.MethodId);
                get.Method = method.Name;

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(ReferenceChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.PaymentOrderId);
                get.Reference = e.Reference;

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Started e)
        {
            using (var session = _store.OpenSession())
            {
                var get = new Responses.Get
                {
                    Id = e.PaymentOrderId,
                    Identity = e.Identity,
                    State = Responses.STATE.New
                };

                session.Store(get);
                session.SaveChanges();
            }
        }
    }
}