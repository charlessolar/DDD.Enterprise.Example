using Demo.Domain.Accounting.Journal.Entities.Entry.Events;
using NServiceBus;
using Raven.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Entry.Handlers
{
    public class Index :
        IHandleMessages<Aborted>,
        IHandleMessages<Closed>,
        IHandleMessages<Exception>,
        IHandleMessages<ItemAdded>,
        IHandleMessages<ItemRemoved>,
        IHandleMessages<Opened>,
        IHandleMessages<Reviewed>,
        IHandleMessages<ReviewRequested>,
        IHandleMessages<Demo.Domain.HumanResources.Employee.Events.FullNameUpdated>
    {
        private readonly IDocumentStore _store;

        public Index(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Aborted e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EntryId);
                index.State = Responses.STATE.Aborted;

                session.SaveChanges();
            }
        }

        public void Handle(Closed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EntryId);
                index.Open = false;
                index.State = Responses.STATE.Closed;

                session.SaveChanges();
            }
        }

        public void Handle(Exception e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EntryId);
                index.State = Responses.STATE.Exception;

                session.SaveChanges();
            }
        }

        public void Handle(ItemAdded e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EntryId);
                index.Items.Add(e.ItemId);

                var item = session.Load<Item.Responses.Get>(e.ItemId);
                if (item.Amount < 0)
                    index.Debits += System.Math.Abs(item.Amount);
                if (item.Amount > 0)
                    index.Credits += System.Math.Abs(item.Amount);

                session.SaveChanges();
            }
        }

        public void Handle(ItemRemoved e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.ItemId);
                index.Items.Remove(e.ItemId);

                var item = session.Load<Item.Responses.Get>(e.ItemId);
                if (item.Amount < 0)
                    index.Debits -= System.Math.Abs(item.Amount);
                if (item.Amount > 0)
                    index.Credits -= System.Math.Abs(item.Amount);

                session.SaveChanges();
            }
        }

        public void Handle(Opened e)
        {
            using (var session = _store.OpenSession())
            {
                var index = new Responses.Index
                {
                    Id = e.EntryId,
                    Open = true,
                    State = Responses.STATE.New,
                };

                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Reviewed e)
        {
            using (var session = _store.OpenSession())
            {
                var employee = session.Load<HumanResources.Employee.Responses.Index>(e.EmployeeId);
                var index = session.Load<Responses.Index>(e.EntryId);
                index.NeedsReview = false;

                index.ReviewEmployee = employee.FullName;

                session.SaveChanges();
            }
        }

        public void Handle(ReviewRequested e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.EntryId);
                index.NeedsReview = true;
                

                session.SaveChanges();
            }
        }
        public void Handle(Demo.Domain.HumanResources.Employee.Events.FullNameUpdated e)
        {
            using (var session = _store.OpenSession())
            {

            }
        }
    }
}