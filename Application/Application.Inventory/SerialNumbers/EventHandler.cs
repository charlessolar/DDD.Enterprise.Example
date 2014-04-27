using Domain.Inventory.SerialNumbers.Events;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.SerialNumbers
{
    public class EventHandler : IHandleMessages<Created>, IHandleMessages<QuantityTaken>
    {
        private readonly IDocumentStore _store;

        public EventHandler(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Created e)
        {
            var serial = new SerialNumber
            {
                Id = e.SerialNumberId,
                Serial = e.SerialNumber,
                Quantity = e.Quantity,
                Effective = e.Effective,
                ItemId = e.ItemId,
            };

            using (IDocumentSession session = _store.OpenSession())
            {
                session.Store(serial);
                session.SaveChanges();
            }
        }

        public void Handle(QuantityTaken e)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var item = session.Load<SerialNumber>(e.SerialNumberId);
                item.Quantity -= e.Quantity;
                session.Store(item);
                session.SaveChanges();
            }
        }
    }
}