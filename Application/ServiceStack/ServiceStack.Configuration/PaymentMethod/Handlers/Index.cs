using Demo.Domain.Configuration.PaymentMethod.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.PaymentMethod.Handlers
{
    public class Index :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>
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
                    Id = e.PaymentMethodId,
                    Name = e.Name,
                    Description = e.Description,
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
                var index = session.Load<Responses.Index>(e.PaymentMethodId);

                session.Delete(index);
                session.SaveChanges();
            }
        }
    }
}