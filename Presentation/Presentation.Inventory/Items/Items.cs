using Application.Inventory.Models;
using NServiceBus;
using Presentation.Inventory.Items.Models;
using Raven.Client;
using Raven.Client.Document;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Inventory.Items
{
    public class Items : Service
    {
        public IDocumentStore _store { get; set; }
        public IBus _bus { get; set; }


        public Item Any(GetItem request)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                return session.Load<Item>(request.Id);
            }
        }

        public List<Item> Any(FindItems request)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                return session.Query<Item>().Where(i => i.Number.StartsWith(request.Number) || i.Description.StartsWith(request.Description)).ToList();
            }
        }

        public Guid Post(CreateItem request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.Commands.Create>();
            command.ItemId = Guid.NewGuid();
            _bus.Send("domain", command);

            return command.ItemId;
        }
    }
}