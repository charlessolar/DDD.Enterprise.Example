using Demo.Domain.Accounting.Currency.Events;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Currency.Handlers
{
    public class Rate :
        IHandleMessages<RateAdded>,
        IHandleMessages<RateEffectiveChanged>
    {
        private readonly IDocumentStore _store;

        public Rate(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(RateAdded e)
        {
            using (var session = _store.OpenSession())
            {
                var rate = session.Load<Responses.Get>(e.CurrencyId);
                var destination = session.Load<Responses.Get>(e.DestinationCurrencyId);

                var dto = new Responses.Rate { Id = e.RateId, Exchange = rate.Code + "/" + destination.Code, CurrencyId = e.CurrencyId, DestinationCurrencyId = e.DestinationCurrencyId, Factor = e.Factor, EffectiveTill = e.EffectiveTill };

                session.Store(dto);
                session.SaveChanges();
            }
        }
        public void Handle(RateEffectiveChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var rate = session.Load<Responses.Rate>(e.RateId);

                rate.EffectiveTill = e.EffectiveTill;

                session.SaveChanges();
            }
        }
    }
}