using Demo.Domain.Accounting.Account.Events;
using Demo.Library.Extensions;
using Demo.Library.SSE;
using Nest;
using NServiceBus;
using System;
using System.Linq;

namespace Demo.Application.ServiceStack.Accounting.Account.Handlers
{
    public class Index :
        IHandleMessages<Created>,
        IHandleMessages<Destroyed>,
        IHandleMessages<NameChanged>,
        IHandleMessages<ParentChanged>,
        IHandleMessages<TypeChanged>,
        IHandleMessages<Frozen>,
        IHandleMessages<Unfrozen>,
        IHandleMessages<Demo.Domain.Configuration.AccountType.Events.NameChanged>
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
            var results = _elastic.MultiGet(m => m
                    .Get<Currency.Responses.Index>(x => x.Id(e.CurrencyId))
                    );

            var currency = results.Get<Currency.Responses.Index>(e.CurrencyId);

            var index = new Responses.Index
            {
                Id = e.AccountId,
                Code = e.Code,
                Name = e.Name,
                Operation = e.Operation,
                Currency = currency.Source.Code,
                CurrencyId = currency.Source.Id
            };

            _elastic.Index(index);
            _manager.Publish(index, ChangeType.NEW);
        }

        public void Handle(Demo.Domain.Configuration.AccountType.Events.NameChanged e)
        {
            var results = _elastic.Search<Responses.Index>(x => x
                .Query(q => q.Term(t => t.TypeId, e.AccountTypeId)));

            foreach (var result in results.Documents.Select(x => x.Id))
            {
                _elastic.Update<Responses.Index, Object>(x => x
                    .Id(result)
                    .Doc(new { Type = e.Name })
                    .RetryOnConflict(3)
                    .Refresh()
                    );
            }

            _manager.PublishAll(results.Documents);
        }

        public void Handle(Destroyed e)
        {
            var index = _elastic.Get<Responses.Index>(e.AccountId);
            _elastic.Delete<Responses.Index>(e.AccountId);

            _manager.Publish(index.Source, ChangeType.DELETE);
        }

        public void Handle(NameChanged e)
        {
            _elastic.Update<Responses.Index, Object>(x => x.
                Id(e.AccountId)
                .Doc(new { Name = e.Name })
                .RetryOnConflict(3)
                .Refresh()
                );

            var results = _elastic.Search<Responses.Index>(x => x
                .Query(q => q.Term(t => t.ParentId, e.AccountId)));

            foreach (var result in results.Documents.Select(x => x.Id))
            {
                _elastic.Update<Responses.Index, Object>(x => x
                    .Id(result)
                    .Doc(new { Parent = e.Name })
                    .RetryOnConflict(3)
                    .Refresh()
                    );
            }

            var index = _elastic.Get<Responses.Index>(e.AccountId);
            _manager.PublishAll(results.Documents);
            _manager.Publish(index.Source);
        }

        public void Handle(TypeChanged e)
        {
            if (e.TypeId.HasValue)
            {
                var type = _elastic.Get<Configuration.AccountType.Responses.Index>(e.TypeId.Value).Source;

                _elastic.Update<Responses.Index, Object>(x => x
                    .Id(e.AccountId)
                    .Doc(new { TypeId = e.TypeId, Type = type.Name })
                    .RetryOnConflict(3)
                    .Refresh()
                    );
            }
            else
            {
                _elastic.Update<Responses.Index, Object>(x => x
                    .Id(e.AccountId)
                    .Doc(new { TypeId = (Guid?)null, Type = "" })
                    .RetryOnConflict(3)
                    .Refresh()
                    );
            }

            var index = _elastic.Get<Responses.Index>(e.AccountId);
            _manager.Publish(index.Source);
        }

        public void Handle(ParentChanged e)
        {
            if (e.ParentId.HasValue)
            {
                var parent = _elastic.Get<Responses.Index>(e.ParentId.Value).Source;

                var parentName = parent.Name;
                var parentCode = parent.Code;
                while (parent.ParentId.HasValue)
                {
                    parent = _elastic.Get<Responses.Index>(parent.ParentId.Value).Source;
                    parentCode = parent.Code + parentCode;
                }

                _elastic.Update<Responses.Index, Object>(x => x
                    .Id(e.AccountId)
                    .Doc(new { ParentCode = parentCode, ParentId = e.ParentId, Parent = parentName })
                    .RetryOnConflict(3)
                    .Refresh()
                    );
            }
            else
            {
                _elastic.Update<Responses.Index, Object>(x => x
                    .Id(e.AccountId)
                    .Doc(new { ParentCode = "", ParentId = (Guid?)null, Parent = "" })
                    .RetryOnConflict(3)
                    .Refresh()
                    );
            }

            var index = _elastic.Get<Responses.Index>(e.AccountId);
            _manager.Publish(index.Source);
        }

        public void Handle(Frozen e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.AccountId)
                .Doc(new { Frozen = true })
                .RetryOnConflict(3)
                .Refresh()
                );

            var index = _elastic.Get<Responses.Index>(e.AccountId);
            _manager.Publish(index.Source);
        }

        public void Handle(Unfrozen e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.AccountId)
                .Doc(new { Frozen = false })
                .RetryOnConflict(3)
                .Refresh()
                );

            var index = _elastic.Get<Responses.Index>(e.AccountId);
            _manager.Publish(index.Source);
        }
    }
}