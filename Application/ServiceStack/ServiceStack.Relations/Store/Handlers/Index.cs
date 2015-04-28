using Demo.Domain.Relations.Store.Events;
using Demo.Library.Extensions;
using Demo.Library.SSE;
using Nest;
using NServiceBus;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Handlers
{
    public class Index :
        IHandleMessages<AddressUpdated>,
        IHandleMessages<Created>,
        IHandleMessages<NameUpdated>,
        IHandleMessages<Destroyed>
    {
        private readonly IElasticClient _elastic;
        private readonly ISubscriptionManager _manager;

        public Index(IElasticClient elastic, ISubscriptionManager manager)
        {
            _elastic = elastic;
            _manager = manager;
        }

        public void Handle(Created e)
        {
            var index = new Responses.Index
            {
                Id = e.StoreId,
                Name = e.Name,
                Identity = e.Identity,
            };
            var status = _elastic.Index(index);
        }

        public void Handle(Destroyed e)
        {
            _elastic.Delete<Responses.Index>(e.StoreId);
        }

        public void Handle(AddressUpdated e)
        {
            var country = _elastic.Get<Configuration.Country.Responses.Index>(e.CountryId);

            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.StoreId)
                .Doc(new { Address = "{0}, {1} {2}".Fmt(e.City, e.District, country.Source.Name) })
                .RetryOnConflict(3)
                .Refresh()
                );
        }

        public void Handle(NameUpdated e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.StoreId)
                .Doc(new { Name = e.Name })
                .RetryOnConflict(3)
                .Refresh()
                );
        }
    }
}