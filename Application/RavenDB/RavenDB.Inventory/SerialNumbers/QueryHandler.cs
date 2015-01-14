using Demo.Library.Queries;
using NServiceBus;
using Raven.Client;
using System;
using System.Linq;

namespace Demo.Application.RavenDB.Inventory.SerialNumbers
{
    public class QueryHandler : IHandleMessages<Queries.All>, IHandleMessages<Queries.Find>, IHandleMessages<Queries.Get>
    {
        private readonly IDocumentStore _store;
        private readonly IBus _bus;

        public QueryHandler(IDocumentStore store, IBus bus)
        {
            _store = store;
            _bus = bus;
        }

        public void Handle(Queries.All query)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var results = session.Query<SerialNumber>()
                    .Skip((query.Page - 1) * query.PageSize).Take(query.PageSize)
                    //.SelectPartialNoDynamic(query.Fields)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Result>(e =>
                {
                    e.Records = results;
                });
            }
        }

        public void Handle(Queries.Find query)
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
                    //.SelectPartialNoDynamic(query.Fields)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Result>(e =>
                {
                    e.Records = results;
                });
            }
        }

        public void Handle(Queries.Get query)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var serial = session.Load<SerialNumber>(query.Id);
                if (serial == null) return; // Return "Unknown serial" or something?

                _bus.Reply<Result>(e =>
                {
                    e.Records = new[] { serial /*.ToPartial(query.Fields)*/ };
                });
            }
        }
    }
}