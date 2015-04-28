using Demo.Domain.Configuration.Country.Events;
using Demo.Library.Extensions;
using Demo.Library.SSE;
using Nest;
using NServiceBus;
using System;

namespace Demo.Application.ServiceStack.Configuration.Country.Handlers
{
    public class Index :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>,
        IHandleMessages<NameUpdated>
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
                Id = e.CountryId,
                Code = e.Code,
                Name = e.Name
            };
            var status = _elastic.Index(index);
        }

        public void Handle(Destroyed e)
        {
            _elastic.Delete<Responses.Index>(e.CountryId);
        }

        public void Handle(NameUpdated e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.CountryId)
                .Doc(new { Name = e.Name })
                .RetryOnConflict(3)
                .Refresh()
                );
        }
    }
}