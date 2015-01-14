using Demo.Library.Queries;
using NServiceBus;
using Raven.Client;
using System;
using System.Linq;

namespace Demo.Application.RavenDB.Inventory.Items
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
                var results = session.Query<Item>()
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
                var store = session.Query<Item>().AsQueryable();

                if (!String.IsNullOrEmpty(query.Number))
                    store = store.Where(x => x.Number.StartsWith(query.Number));
                if (!String.IsNullOrEmpty(query.Description))
                    store = store.Where(x => x.Number.StartsWith(query.Description));

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
                var item = session.Load<Item>(query.Id);
                if (item == null) return; // Return "Unknown item" or something?

                _bus.Reply<Result>(e =>
                {
                    e.Records = new[] { item /*.ToPartial(query.Fields)*/ };
                });
            }
        }
    }
}