using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aggregates;
using EventStore.ClientAPI;
using Raven.Client;

namespace Demo.Application.Servicestack.RavenDB
{
    public class RavenCheckpointPersister : IPersistCheckpoints
    {
        private readonly IDocumentStore _store;

        public RavenCheckpointPersister(IDocumentStore store)
        {
            _store = store;
        }

        public Position Load(String endpoint)
        {
            using (var session = _store.OpenSession())
            {
                var position = session.Load<Position?>(endpoint);
                return position ?? Position.Start;
            }
        }

        public void Save(String endpoint, Position position)
        {
            using (var session = _store.OpenSession())
            {
                session.Store(position, endpoint);
                session.SaveChanges();
            }
        }
    }
}