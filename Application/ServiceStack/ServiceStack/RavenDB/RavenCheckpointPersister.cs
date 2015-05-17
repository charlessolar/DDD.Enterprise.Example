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

        public EventStore.ClientAPI.Position Load(String endpoint)
        {
            using (var session = _store.OpenSession())
            {
                var position = session.Load<Position>(endpoint);
                if (position == null) return EventStore.ClientAPI.Position.Start;

                return new EventStore.ClientAPI.Position(position.CommitPosition, position.PreparePosition);
            }
        }

        public void Save(String endpoint, EventStore.ClientAPI.Position position)
        {
            using (var session = _store.OpenSession())
            {
                var db = session.Load<Position>(endpoint);

                if (db == null)
                {
                    db = new Position { Id = endpoint };
                    session.Store(db);
                }

                db.CommitPosition = position.CommitPosition;
                db.PreparePosition = position.PreparePosition;

                session.SaveChanges();
            }
        }
    }
}