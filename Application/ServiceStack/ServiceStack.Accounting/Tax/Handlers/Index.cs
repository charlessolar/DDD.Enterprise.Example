using Demo.Domain.Accounting.Tax.Events;
using NServiceBus;
using Raven.Client;

namespace Demo.Application.ServiceStack.Accounting.Tax.Handlers
{
    public class Index :
        IHandleMessages<AccountChanged>,
        IHandleMessages<Activated>,
        IHandleMessages<Created>,
        IHandleMessages<Deactivated>,
        IHandleMessages<Destroyed>,
        IHandleMessages<NameChanged>,
        IHandleMessages<RateChanged>
    {
        private readonly IDocumentStore _store;

        public Index(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(AccountChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var account = session.Load<Accounting.Account.Responses.Index>(e.AccountId);
                var index = session.Load<Responses.Index>(e.TaxId);
                index.Account = account.Code;

                session.SaveChanges();
            }
        }

        public void Handle(Activated e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.TaxId);
                index.Activated = true;

                session.SaveChanges();
            }
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var type = session.Load<Configuration.TaxType.Responses.Index>(e.TaxTypeId);
                var index = new Responses.Index
                {
                    Id = e.TaxId,
                    Code = e.Code,
                    Name = e.Name,
                    TypeId = e.TaxTypeId,
                    Type = type.Name,
                };

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Deactivated e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.TaxId);
                index.Activated = false;

                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.TaxId);

                session.Delete(index);
                session.SaveChanges();
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.TaxId);
                index.Name = e.Name;

                session.SaveChanges();
            }
        }

        public void Handle(RateChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.TaxId);
                index.Fixed = e.Fixed;
                index.Rate = e.Rate;

                session.SaveChanges();
            }
        }
    }
}