using Demo.Domain.Accounting.Tax.Events;
using NServiceBus;
using Raven.Client;
using System.Linq;

namespace Demo.Application.ServiceStack.Accounting.Tax.Handlers
{
    public class Get :
        IHandleMessages<AccountChanged>,
        IHandleMessages<Activated>,
        IHandleMessages<Created>,
        IHandleMessages<Deactivated>,
        IHandleMessages<DescriptionChanged>,
        IHandleMessages<Destroyed>,
        IHandleMessages<NameChanged>,
        IHandleMessages<RateChanged>,
        IHandleMessages<Demo.Domain.Configuration.TaxType.Events.NameChanged>
    {
        private readonly IDocumentStore _store;

        public Get(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(AccountChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var account = session.Load<Accounting.Account.Responses.Index>(e.AccountId);
                var get = session.Load<Responses.Get>(e.TaxId);
                get.AccountId = e.AccountId;
                get.Account = account.Code;

                session.SaveChanges();
            }
        }

        public void Handle(Activated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.TaxId);
                get.Activated = true;

                session.SaveChanges();
            }
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var taxType = session.Load<Configuration.TaxType.Responses.Index>(e.TaxTypeId);
                var get = new Responses.Get
                {
                    Id = e.TaxId,
                    Code = e.Code,
                    Description = e.Description,
                    Name = e.Name,
                    TaxTypeId = e.TaxTypeId,
                    Type = taxType.Name,
                };

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(Demo.Domain.Configuration.TaxType.Events.NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var gets = session.Query<Responses.Get>().Where(x => x.TaxTypeId == e.TaxTypeId).ToList();
                foreach (var get in gets)
                    get.Type = e.Name;

                session.SaveChanges();
            }
        }

        public void Handle(Deactivated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.TaxId);
                get.Activated = false;

                session.SaveChanges();
            }
        }

        public void Handle(DescriptionChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.TaxId);
                get.Description = e.Description;

                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.TaxId);

                session.Delete(get);
                session.SaveChanges();
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.TaxId);
                get.Name = e.Name;

                session.SaveChanges();
            }
        }

        public void Handle(RateChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.TaxId);
                get.Fixed = e.Fixed;
                get.Rate = e.Rate;

                session.SaveChanges();
            }
        }
    }
}