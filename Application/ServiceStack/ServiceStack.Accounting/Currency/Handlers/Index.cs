using Demo.Domain.Accounting.Currency.Events;
using Demo.Library.Extensions;
using Demo.Library.SSE;
using Nest;
using NServiceBus;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Handlers
{
    public class Index :
        IHandleMessages<Activated>,
        IHandleMessages<Created>,
        IHandleMessages<Deactivated>,
        IHandleMessages<NameChanged>,
        IHandleMessages<SymbolChanged>,
        IHandleMessages<FormatChanged>
    {
        private readonly IElasticClient _elastic;
        private readonly ISubscriptionManager _manager;

        public Index(IElasticClient elastic, ISubscriptionManager manager)
        {
            _elastic = elastic;
            _manager = manager;
        }

        public void Handle(Activated e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.CurrencyId)
                .Doc(new { Activated = true })
                .RetryOnConflict(3)
                .Refresh()
                );

            var index = _elastic.Get<Responses.Index>(e.CurrencyId);
            _manager.Publish(index.Source);
        }

        public void Handle(Created e)
        {
            var index = new Responses.Index
            {
                Id = e.CurrencyId,
                Code = e.Code,
                Name = e.Name,
                Symbol = e.Symbol,
                Format = e.Format,
            };

            _elastic.Index(index);
            _manager.Publish(index, ChangeType.NEW);
        }

        public void Handle(Deactivated e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.CurrencyId)
                .Doc(new { Activated = false })
                .RetryOnConflict(3)
                .Refresh()
                );

            var index = _elastic.Get<Responses.Index>(e.CurrencyId);
            _manager.Publish(index.Source);
        }

        public void Handle(NameChanged e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.CurrencyId)
                .Doc(new { Name = e.Name })
                .RetryOnConflict(3)
                .Refresh()
                );

            var index = _elastic.Get<Responses.Index>(e.CurrencyId);
            _manager.Publish(index.Source);
        }

        public void Handle(FormatChanged e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.CurrencyId)
                .Doc(new { Format = e.Format })
                .RetryOnConflict(3)
                .Refresh()
                );

            var index = _elastic.Get<Responses.Index>(e.CurrencyId);
            _manager.Publish(index.Source);
        }

        public void Handle(SymbolChanged e)
        {
            _elastic.Update<Responses.Index, Object>(x => x
                .Id(e.CurrencyId)
                .Doc(new { Symbol = e.Symbol })
                .RetryOnConflict(3)
                .Refresh()
                );

            var index = _elastic.Get<Responses.Index>(e.CurrencyId);
            _manager.Publish(index.Source);
        }
    }
}