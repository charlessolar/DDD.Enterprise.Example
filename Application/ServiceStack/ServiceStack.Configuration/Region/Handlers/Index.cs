using Demo.Domain.Configuration.Region.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Handlers
{
    public class Index :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>,
        IHandleMessages<DescriptionChanged>,
        IHandleMessages<NameChanged>
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
                    Id = e.RegionId,
                    Code = e.Code,
                    Name = e.Name,
                    ParentId = e.ParentId
                };
                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.RegionId);

                session.Delete(index);
                session.SaveChanges();
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.RegionId);
                index.Name = e.Name;

                session.SaveChanges();
            }
        }

        public void Handle(DescriptionChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.RegionId);
                index.Description = e.Description;

                session.SaveChanges();
            }
        }
    }
}