using Demo.Domain.Relations.Store.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store.Handlers
{
    public class Warehouse :
        IHandleMessages<WarehouseAdded>,
        IHandleMessages<WarehouseRemoved>
    {
        private readonly IDocumentStore _store;

        public Warehouse(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(WarehouseAdded e)
        {
            using (var session = _store.OpenSession())
            {
                var store = session.Load<Configuration.Region.Responses.Index>(e.StoreId);
                var warehouse = session.Load<ServiceStack.Warehouse.Warehouse.Responses.Index>(e.WarehouseId);
                session.Store(new Responses.Warehouse { Id = Guid.NewGuid(), StoreId = e.StoreId, Identity = warehouse.Identity, WarehouseId = warehouse.Id });
                session.SaveChanges();
            }
        }

        public void Handle(WarehouseRemoved e)
        {
            using (var session = _store.OpenSession())
            {
                var response = session.Query<Responses.Warehouse>().Single(x => x.StoreId == e.StoreId && x.WarehouseId == e.WarehouseId);

                session.Delete(response);

                session.SaveChanges();
            }
        }
    }
}