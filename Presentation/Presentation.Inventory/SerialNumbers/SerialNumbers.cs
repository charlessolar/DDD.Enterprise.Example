using Application.Inventory.Models;
using NServiceBus;
using Presentation.Inventory.SerialNumbers.Models;
using Raven.Client;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Presentation.Inventory.SerialNumbers
{
    public class SerialNumbers : Service
    {
        public IDocumentStore _store { get; set; }
        public IBus _bus { get; set; }

        public SerialNumber Any(GetSerialNumber request)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                return session.Load<SerialNumber>(request.Id);
            }
        }

        public List<SerialNumber> Any(FindSerialNumber request)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                return session.Query<SerialNumber>().Where(i => i.Serial.StartsWith(request.Serial) || i.Effective == request.Effective || i.ItemId == request.ItemId).ToList();
            }
        }

        public SerialNumber Post(CreateSerialNumber request)
        {
            var command = request.ConvertTo<Domain.Inventory.SerialNumbers.Commands.Create>();
            command.ItemId = Guid.NewGuid();
            _bus.Send("domain", command);

            // I don't know!
            // Servicestack wants an object back - Im not sure if that's required yet, but in case Ill just sleep until raven has it
            Thread.Sleep(1000);

            return Any(new GetSerialNumber { Id = command.ItemId });
        }
    }
}