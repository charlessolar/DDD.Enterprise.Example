using Demo.Domain.Configuration.AccountType.Events;
using Demo.Library.Extensions;
using Demo.Library.SSE;
using Nest;
using NServiceBus;
using System;

namespace Demo.Application.ServiceStack.Configuration.AccountType.Handlers
{
    public class Index :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>,
        IHandleMessages<NameChanged>,
        IHandleMessages<DeferralChanged>
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
                Id = e.AccountTypeId,
                Name = e.Name,
                DeferralMethod = e.DeferralMethod,
                ParentId = e.ParentId,
            };
            var status = _elastic.Index(index);
        }

        public void Handle(Destroyed e)
        {
            _elastic.Delete<Responses.Index>(e.AccountTypeId);
        }

        public void Handle(NameChanged e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.AccountTypeId)
                .Doc(new { Name = e.Name })
                .RetryOnConflict(3)
                .Refresh()
                );
        }

        public void Handle(DeferralChanged e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.AccountTypeId)
                .Doc(new { DeferralMethod = e.DeferralMethod })
                .RetryOnConflict(3)
                .Refresh()
                );
        }
    }
}