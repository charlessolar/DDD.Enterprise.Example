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
        public IDocumentStore _store { get; set; }
        public IBus _bus { get; set; }

        public void Handle(Queries.AllItems command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                session.Query<Item>().Skip((command.Page - 1) * command.PageSize).Take(command.PageSize)
                    .ToList()
                    .ForEach(x =>
                    {
                        _bus.Reply<Messages.ItemRetreived>(e => e.Item = x);
                    });
            }
        }
        public void Handle(Queries.FindItems command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                session.Query<Item>().Where(x => x.Number.StartsWith(command.Number) || x.Description.StartsWith(command.Description))
                    .ToList()
                    .ForEach(x =>
                    {
                        _bus.Reply<Messages.ItemRetreived>(e => e.Item = x);
                    });
            }
        }
        public void Handle(Queries.GetItem command)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var serial = session.Load<Item>(command.Id);
                _bus.Reply<Messages.ItemRetreived>(e => e.Item = serial);
            }
        }
    }
}