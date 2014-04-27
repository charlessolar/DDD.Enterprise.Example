using Library.Extenstions;
using Library.Queries;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Inventory.Items
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

        public void Handle(Queries.AllItems command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var results = session.Query<Item>()
                    .Skip((command.Page - 1) * command.PageSize).Take(command.PageSize)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Messages.ItemsRetreived>(e =>
                    {
                        e.Items = results;
                    });
            }
        }
        public void Handle(Queries.FindItems command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var query = session.Query<Item>().AsQueryable();

                if (!String.IsNullOrEmpty(command.Number))
                    query = query.Where(x => x.Number.StartsWith(command.Number));
                if (!String.IsNullOrEmpty(command.Description))
                    query = query.Where(x => x.Number.StartsWith(command.Description));

                var results = query
                    .Skip((command.Page - 1) * command.PageSize).Take(command.PageSize)
                    .ToList();

                _bus.CurrentMessageContext.Headers["Count"] = results.Count.ToString();

                _bus.Reply<Messages.ItemsRetreived>(e =>
                {
                    e.Items = results;
                });
            }
        }
        public void Handle(Queries.GetItem command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var item = session.Load<Item>(command.Id);
                if (item == null) return; // Return "Unknown item" or something?

                _bus.Reply<Messages.ItemsRetreived>(e =>
                {
                    e.Items = new[] { item };
                });
            }
        }
    }
}