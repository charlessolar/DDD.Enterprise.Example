using Demo.Domain.HumanResources.Employee.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Handlers
{
    public class Index :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>,
        IHandleMessages<FullNameUpdated>,
        IHandleMessages<GenderUpdated>,
        IHandleMessages<Hired>,
        IHandleMessages<PhoneUpdated>,
        IHandleMessages<Terminated>
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
                var index = new Responses.Index
                {
                    Id = e.EmployeeId,
                    FullName = e.FullName,
                    Identity = e.Identity,
                };
                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EmployeeId);

                session.Delete(index);
                session.SaveChanges();
            }
        }

        public void Handle(FullNameUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EmployeeId);
                index.FullName = e.FullName;

                session.SaveChanges();
            }
        }

        public void Handle(GenderUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EmployeeId);
                index.Gender = e.Gender.DisplayName;

                session.SaveChanges();
            }
        }

        public void Handle(Hired e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EmployeeId);
                index.Hired = e.Effective;

                session.SaveChanges();
            }
        }

        public void Handle(PhoneUpdated e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EmployeeId);
                index.Phone = e.Phone;

                session.SaveChanges();
            }
        }

        public void Handle(Terminated e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EmployeeId);
                index.Terminated = e.Effective;

                session.SaveChanges();
            }
        }
    }
}