using Demo.Library.Extensions;
using Demo.Library.Queries;
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

        public void Handle(Queries.AllSerialNumbers query)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var results = session.Query<SerialNumber>()
                    .Skip((query.Page - 1) * query.PageSize).Take(query.PageSize)
                    .SelectPartial(query.Fields)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Result>(e =>
                {
                    e.Records = results;
                });
            }
        }
        public void Handle(Queries.FindSerialNumbers query)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var store = session.Query<SerialNumber>().AsQueryable();
                if (!String.IsNullOrEmpty(query.Serial))
                    store = store.Where(x => x.Serial.StartsWith(query.Serial));
                if (query.Effective.HasValue)
                    store = store.Where(x => x.Effective == query.Effective);
                if (query.ItemId.HasValue)
                    store = store.Where(x => x.ItemId == query.ItemId);

                var results = store
                    .Skip((query.Page - 1) * query.PageSize).Take(query.PageSize)
                    .SelectPartial(query.Fields)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Result>(e =>
                {
                    e.Records = results;
                });
            }
        }
        public void Handle(Queries.GetSerialNumber query)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var serial = session.Load<SerialNumber>(query.Id);
                if (serial == null) return; // Return "Unknown serial" or something?

                _bus.Reply<Result>(e =>
                {
                    e.Records = new[] { serial.ToPartial(query.Fields) };
                });
            }
        }
    }
}