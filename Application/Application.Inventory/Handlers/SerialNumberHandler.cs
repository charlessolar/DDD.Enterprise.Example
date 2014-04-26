using Application.Inventory.Models;
using Domain.Inventory.SerialNumbers.Events;
using NServiceBus;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.Handlers
{
    public class SerialNumberHandler : IHandleMessages<Created>, IHandleMessages<QuantityTaken>
    {
        private IDocumentStore _store { get; set; }

        public SerialNumberHandler()
        {
            _store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "Demo-ReadModels" };
            _store.Initialize();
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