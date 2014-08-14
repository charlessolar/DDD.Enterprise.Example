using Demo.Library.Extensions;
using Demo.Library.Queries;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Inventory.Items
{
    public class QueryHandler : IHandleMessages<Queries.AllItems>, IHandleMessages<Queries.FindItems>, IHandleMessages<Queries.GetItem>
    {
        private readonly IDocumentStore _store;
        private readonly IBus _bus;

        public QueryHandler(IDocumentStore store, IBus bus)
        {
            _store = store;
            _bus = bus;
        }

        public void Handle(Queries.AllItems query)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var results = session.Query<Item>()
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
        public void Handle(Queries.FindItems query)
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
                    .SelectPartial(query.Fields)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Result>(e =>
                {
                    e.Records = results;
                });
            }
        }
        public void Handle(Queries.GetItem query)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var item = session.Load<Item>(query.Id);
                if (item == null) return; // Return "Unknown item" or something?

                _bus.Reply<Result>(e =>
                {
                    e.Records = new[] { item.ToPartial(query.Fields) };
                });
            }
        }
    }
}