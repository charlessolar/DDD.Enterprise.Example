using Demo.Domain.Accounting.Journal.Events;
using NServiceBus;
using Raven.Client;
using System.Linq;

namespace Demo.Application.ServiceStack.Accounting.Journal.Handlers
{
    public class Get :
        IHandleMessages<CheckDateChanged>,
        IHandleMessages<Created>,
        IHandleMessages<CreditAccountSet>,
        IHandleMessages<DebitAccountSet>,
        IHandleMessages<Destroyed>,
        IHandleMessages<NameChanged>,
        IHandleMessages<ResponsibleChanged>,
        IHandleMessages<SkipDraftChanged>,
        IHandleMessages<Closed>,
        IHandleMessages<Opened>,
        IHandleMessages<Demo.Domain.HumanResources.Employee.Events.FullNameUpdated>
    {
        private readonly IDocumentStore _store;

        public Get(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(CheckDateChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.JournalId);
                get.CheckDate = e.CheckDate;

                session.SaveChanges();
            }
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var employee = session.Load<HumanResources.Employee.Responses.Index>(e.ResponsibleId);

                var get = new Responses.Get
                {
                    Id = e.JournalId,
                    Code = e.Code,
                    Name = e.Name,
                    Responsible = employee.FullName,
                    ResponsibleId = e.ResponsibleId,
                    CheckDate = e.CheckDate,
                    SkipDraft = e.SkipDraft
                };

                session.Store(get);
                session.SaveChanges();
            }
        }

        public void Handle(CreditAccountSet e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.JournalId);
                get.CreditAccountId = e.AccountId;
                get.CreditAccount = "";
                if (e.AccountId.HasValue)
                {
                    var account = session.Load<Account.Responses.Index>(e.AccountId);
                    get.CreditAccount = account.Code;
                }

                session.SaveChanges();
            }
        }

        public void Handle(DebitAccountSet e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.JournalId);
                get.DebitAccountId = e.AccountId;
                get.DebitAccount = "";
                if (e.AccountId.HasValue)
                {
                    var account = session.Load<Account.Responses.Index>(e.AccountId);
                    get.DebitAccount = account.Code;
                }

                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.JournalId);

                session.Delete(get);
                session.SaveChanges();
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.JournalId);
                get.Name = e.Name;

                session.SaveChanges();
            }
        }

        public void Handle(Closed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.JournalId);
                get.Closed = true;

                session.SaveChanges();
            }
        }

        public void Handle(Opened e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.JournalId);
                get.Closed = false;

                session.SaveChanges();
            }
        }

        public void Handle(ResponsibleChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var employee = session.Load<HumanResources.Employee.Responses.Index>(e.EmployeeId);
                var get = session.Load<Responses.Get>(e.JournalId);

                get.ResponsibleId = e.EmployeeId;
                get.Responsible = employee.FullName;

                session.SaveChanges();
            }
        }

        public void Handle(Demo.Domain.HumanResources.Employee.Events.FullNameUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var gets = session.Query<Responses.Get>().Where(x => x.ResponsibleId == e.EmployeeId).ToList();
                foreach (var get in gets)
                    get.Responsible = e.FullName;

                session.SaveChanges();
            }
        }

        public void Handle(SkipDraftChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.JournalId);
                get.SkipDraft = e.SkipDraft;

                session.SaveChanges();
            }
        }
    }
}