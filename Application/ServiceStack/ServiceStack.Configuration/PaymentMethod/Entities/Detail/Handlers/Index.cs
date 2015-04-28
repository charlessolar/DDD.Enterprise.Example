using Demo.Domain.Configuration.PaymentMethod.Entities.Detail.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.PaymentMethod.Entities.Detail.Handlers
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
                    Id = e.DetailId,
                    PaymentMethodId = e.PaymentMethodId,
                    Name = e.Name,
                    Hint = e.Hint
                };
                session.Store(index);
                session.SaveChanges();
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var index = session.Load<Responses.Index>(e.DetailId);

                session.Delete(index);
                session.SaveChanges();
            }
        }
    }
}