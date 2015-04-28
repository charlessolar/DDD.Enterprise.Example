using Demo.Domain.Configuration.Region.Entities.Match.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Entities.Match.Handlers
{
    public class Index :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>,
        IHandleMessages<OperationChanged>,
        IHandleMessages<TypeChanged>,
        IHandleMessages<ValueChanged>
    {
        private readonly IDocumentStore _store;

        public Index(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var index = new Responses.Index
                {
                    Id = e.MatchId,
                    RegionId = e.RegionId,
                    Type = e.Type.DisplayName,
                    Operation = e.Operation.DisplayName,
                    Value = e.Value
                };
                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.MatchId);

                session.Delete(index);
                session.SaveChanges();
            }
        }

        public void Handle(OperationChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.MatchId);
                index.Operation = e.Operation.DisplayName;

                session.SaveChanges();
            }
        }

        public void Handle(TypeChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.MatchId);
                index.Type = e.Type.DisplayName;

                session.SaveChanges();
            }
        }

        public void Handle(ValueChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.MatchId);
                index.Value = e.Value;

                session.SaveChanges();
            }
        }
    }
}