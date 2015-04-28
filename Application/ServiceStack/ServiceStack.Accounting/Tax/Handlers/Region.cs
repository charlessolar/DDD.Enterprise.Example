using Demo.Domain.Accounting.Tax.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Tax.Handlers
{
    public class Region :
        IHandleMessages<RegionAdded>,
        IHandleMessages<RegionRemoved>
    {
        private readonly IDocumentStore _store;

        public Region(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(RegionAdded e)
        {
            using (var session = _store.OpenSession())
            {
                var tax = session.Load<Accounting.Tax.Responses.Index>(e.TaxId);
                var region = session.Load<Configuration.Region.Responses.Index>(e.RegionId);
                session.Store(new Responses.Region { Id = Guid.NewGuid(), TaxId = tax.Id, RegionId = region.Id, Code = region.Code });
                session.SaveChanges();
            }
        }

        public void Handle(RegionRemoved e)
        {
            using (var session = _store.OpenSession())
            {
                var response = session.Query<Responses.Region>().SingleAsync(x => x.TaxId == e.TaxId && x.RegionId == e.RegionId);
                session.Delete(response);
                session.SaveChanges();
            }
        }
    }
}