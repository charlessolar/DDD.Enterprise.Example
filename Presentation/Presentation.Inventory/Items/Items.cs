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

        public Item Post(CreateItem request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.Commands.Create>();
            command.ItemId = Guid.NewGuid();
            _bus.Send("domain", command);

            // I don't know!
            // Servicestack wants an object back - Im not sure if that's required yet, but in case Ill just sleep until raven has it
            Thread.Sleep(1000);

            return Any(new GetItem { Id = command.ItemId });
        }
    }
}