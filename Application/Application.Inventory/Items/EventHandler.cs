using Domain.Inventory.Items.Events;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.Items
{
    public class EventHandler : IHandleMessages<Created>, IHandleMessages<DescriptionChanged>
    {
        public IDocumentStore _store { get; set; }

        public void Handle(Created e)
        {
            var account = new Item
            {
                Id = e.ItemId,
                Number = e.Number,
                Description = e.Description,
                UnitOfMeasure = e.UnitOfMeasure,
                CatalogPrice = e.CatalogPrice,
                CostPrice = e.CostPrice,
            };

            using (IDocumentSession session = _store.OpenSession())
            {
                session.Store(account);
                session.SaveChanges();
            }
        }

        public void Handle(DescriptionChanged e)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var item = session.Load<Item>(e.ItemId);
                item.Description = e.Description;
                session.Store(item);
                session.SaveChanges();
            }
        }
    }
}