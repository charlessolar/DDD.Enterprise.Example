using Demo.Library.Extenstions;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Inventory.SerialNumbers
{
    public class QueryHandler : IHandleMessages<Queries.AllSerialNumbers>, IHandleMessages<Queries.FindSerialNumbers>, IHandleMessages<Queries.GetSerialNumber>
    {
        private readonly IDocumentStore _store;
        private readonly IBus _bus;

        public QueryHandler(IDocumentStore store, IBus bus)
        {
            _store = store;
            _bus = bus;
        }

        public void Handle(Queries.AllSerialNumbers command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var results = session.Query<SerialNumber>()
                    .Skip((command.Page - 1) * command.PageSize).Take(command.PageSize)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Messages.SerialNumbersRetreived>(e =>
                {
                    e.SerialNumbers = results;
                });
            }
        }
        public void Handle(Queries.FindSerialNumbers command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var query = session.Query<SerialNumber>().AsQueryable();
                if (!String.IsNullOrEmpty(command.Serial))
                    query = query.Where(x => x.Serial.StartsWith(command.Serial));
                if (command.Effective.HasValue)
                    query = query.Where(x => x.Effective == command.Effective);
                if (command.ItemId.HasValue)
                    query = query.Where(x => x.ItemId == command.ItemId);

                var results = query
                    .Skip((command.Page - 1) * command.PageSize).Take(command.PageSize)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Messages.SerialNumbersRetreived>(e =>
                {
                    e.SerialNumbers = results;
                });
            }
        }
        public void Handle(Queries.GetSerialNumber command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var serial = session.Load<SerialNumber>(command.Id);
                if (serial == null) return; // Return "Unknown serial" or something?

                _bus.Reply<Messages.SerialNumbersRetreived>(e =>
                {
                    e.SerialNumbers = new[] { serial };
                });
            }
        }
    }
}