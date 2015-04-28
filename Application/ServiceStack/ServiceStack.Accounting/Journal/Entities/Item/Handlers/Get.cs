using Demo.Domain.Accounting.Journal.Entities.Item.Events;
using NServiceBus;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Item.Handlers
{
    public class Get :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>,
        IHandleMessages<EffectiveChanged>,
        IHandleMessages<Reconciled>,
        IHandleMessages<ReferenceChanged>
    {
        private readonly IDocumentStore _store;

        public Get(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var account = session.Load<Account.Responses.Get>(e.AccountId);
                var journal = session.Load<Journal.Responses.Get>(e.JournalId);
                var period = session.Load<Journal.Responses.Get>(e.PeriodId);

                var get = new Responses.Get
                {
                    Id = e.ItemId,
                    JournalId = e.JournalId,
                    Journal = journal.Code,
                    Effective = e.Effective,
                    Reference = e.Reference,
                    AccountId = e.AccountId,
                    Account = account.Code,
                    PeriodId = e.PeriodId,
                    Period = period.Code,
                    Amount = e.Amount,
                };

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.ItemId);

                session.Delete(get);
                session.SaveChanges();
            }
        }

        public void Handle(EffectiveChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.ItemId);
                get.Effective = e.Effective;

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Reconciled e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.ItemId);
                get.ReconciledAmount += e.Amount;
                get.Reconciled = (get.Amount == get.ReconciledAmount);

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(ReferenceChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.ItemId);
                get.Reference = e.Reference;

                session.Store(get);
                session.SaveChanges();
            }
        }
    }
}