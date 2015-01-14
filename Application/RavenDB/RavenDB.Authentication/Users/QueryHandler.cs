using Demo.Library.Queries;
using NServiceBus;
using Raven.Client;

namespace Demo.Application.RavenDB.Authentication.Users
{
    public class QueryHandler : IHandleMessages<Queries.Get>
    {
        private readonly IDocumentStore _store;
        private readonly IBus _bus;

        public QueryHandler(IDocumentStore store, IBus bus)
        {
            _store = store;
            _bus = bus;
        }

        public void Handle(Queries.Get query)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var user = session.Load<User>(query.Id);
                if (user == null) return; // Return "Unknown item" or something?

                _bus.Reply<Result>(e =>
                {
                    e.Records = new[] { user /*.ToPartial(query.Fields)*/ };
                });
            }
        }
    }
}