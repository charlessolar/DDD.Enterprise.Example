using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.SerialNumbers
{
    public class QueryHandler : IHandleMessages<Queries.AllSerialNumbers>, IHandleMessages<Queries.FindSerialNumbers>, IHandleMessages<Queries.GetSerialNumber>
    {
        public IDocumentStore _store { get; set; }
        public IBus _bus { get; set; }

        public void Handle(Queries.AllSerialNumbers command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                session.Query<SerialNumber>().Skip((command.Page - 1) * command.PageSize).Take(command.PageSize)
                    .ToList()
                    .ForEach(x =>
                    {
                        _bus.Reply<Messages.SerialNumberRetreived>(e => e.SerialNumber = x);
                    });
            }
        }
        public void Handle(Queries.FindSerialNumbers command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                session.Query<SerialNumber>().Where(x => x.Serial.StartsWith(command.Serial) || x.Effective == command.Effective || x.ItemId == command.ItemId)
                    .ToList()
                    .ForEach(x =>
                    {
                        _bus.Reply<Messages.SerialNumberRetreived>(e => e.SerialNumber = x);
                    });
            }
        }
        public void Handle(Queries.GetSerialNumber command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var serial = session.Load<SerialNumber>(command.Id);
                _bus.Reply<Messages.SerialNumberRetreived>(e => e.SerialNumber = serial);
            }
        }
    }
}