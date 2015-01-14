using Demo.Domain.Inventory.Items.SerialNumbers.Events;
using NServiceBus;
using Raven.Client;

namespace Demo.Application.RavenDB.Inventory.SerialNumbers
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