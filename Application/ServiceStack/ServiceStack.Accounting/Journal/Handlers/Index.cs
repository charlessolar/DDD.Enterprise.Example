using Demo.Domain.Accounting.Journal.Events;
using NServiceBus;
using Raven.Client;
using System.Linq;

namespace Demo.Application.ServiceStack.Accounting.Journal.Handlers
{
    public class Index :
        IHandleMessages<Created>,
        IHandleMessages<CreditAccountSet>,
        IHandleMessages<DebitAccountSet>,
        IHandleMessages<Destroyed>,
        IHandleMessages<NameChanged>,
        IHandleMessages<ResponsibleChanged>,
        IHandleMessages<Closed>,
        IHandleMessages<Opened>,
        IHandleMessages<Demo.Domain.HumanResources.Employee.Events.FullNameUpdated>
    {
        private readonly IDocumentStore _store;

        public Index(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var employee = session.Load<HumanResources.Employee.Responses.Index>(e.ResponsibleId);
                var index = new Responses.Index
                {
                    Id = e.JournalId,
                    Code = e.Code,
                    Name = e.Name,
                    Responsible = employee.FullName,
                    ResponsibleId = e.ResponsibleId,
                };

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(CreditAccountSet e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.JournalId);
                index.CreditAccount = "";
                if (e.AccountId.HasValue)
                {
                    var account = session.Load<Account.Responses.Index>(e.AccountId);
                    index.CreditAccount = account.Code;
                }

                session.SaveChanges();
            }
        }

        public void Handle(DebitAccountSet e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.JournalId);
                index.DebitAccount = "";
                if (e.AccountId.HasValue)
                {
                    var account = session.Load<Account.Responses.Index>(e.AccountId);
                    index.DebitAccount = account.Code;
                }

                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.JournalId);

                session.Delete(index);
                session.SaveChanges();
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.JournalId);
                index.Name = e.Name;

                session.SaveChanges();
            }
        }

        public void Handle(Closed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.JournalId);
                index.Closed = true;

                session.SaveChanges();
            }
        }

        public void Handle(Opened e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.JournalId);
                index.Closed = false;

                session.SaveChanges();
            }
        }

        public void Handle(ResponsibleChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var employee = session.Load<HumanResources.Employee.Responses.Index>(e.EmployeeId);
                var index = session.Load<Responses.Index>(e.JournalId);

                index.ResponsibleId = e.EmployeeId;
                index.Responsible = employee.FullName;

                session.SaveChanges();
            }
        }

        public void Handle(Demo.Domain.HumanResources.Employee.Events.FullNameUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var indicies = session.Query<Responses.Index>().Where(x => x.ResponsibleId == e.EmployeeId).ToList();
                foreach (var index in indicies)
                    index.Responsible = e.FullName;

                session.SaveChanges();
            }
        }
    }
}