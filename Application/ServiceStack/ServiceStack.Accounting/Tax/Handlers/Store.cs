using Demo.Domain.Accounting.Tax.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Tax.Handlers
{
    public class Store :
        IHandleMessages<StoreAdded>,
        IHandleMessages<StoreRemoved>
    {
        private readonly IDocumentStore _store;

        public Store(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(StoreAdded e)
        {
            using (var session = _store.OpenSession())
            {
                var tax = session.Load<Accounting.Tax.Responses.Index>(e.TaxId);
                var store = session.Load<Configuration.Region.Responses.Index>(e.StoreId);
                session.Store(new Responses.Store { Id = Guid.NewGuid(), StoreId = e.StoreId, TaxId = tax.Id, Code = store.Code });
                session.SaveChanges();
            }
        }

        public void Handle(StoreRemoved e)
        {
            using (var session = _store.OpenSession())
            {
                var response = session.Query<Responses.Store>().SingleAsync(x => x.TaxId == e.TaxId && x.StoreId == e.StoreId);
                session.Delete(response);
                session.SaveChanges();
            }
        }
    }
}