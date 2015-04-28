using Demo.Library.Extensions;
using Demo.Library.Queries.Processor;
using Raven.Client;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q = Demo.Library.Queries;
using R = Demo.Library.Responses;

namespace Demo.Application.ServiceStack.Accounting.Currency.Queries
{
    public class GetRate : IPagingQueryHandler<Services.GetRate, Responses.Rate>
    {
        private readonly IDocumentStore _store;

        public GetRate(IDocumentStore store)
        {
            _store = store;
        }

        public bool Satisfied(Services.GetRate query, Responses.Rate dto)
        {
            throw new NotImplementedException();
        }

        public R.Query<R.Paged<Responses.Rate>> Handle(Services.GetRate query)
        {
            using (var session = _store.OpenSession())
            {
                var rates = session.Query<Responses.Rate>().Where(x => x.CurrencyId == query.CurrencyId || x.DestinationCurrencyId == query.CurrencyId).OrderByDescending(x => x.EffectiveTill).Take(10).ToList();

                var paged = new R.Paged<Responses.Rate> { Records = rates, Total = rates.Count() };

                return new R.Query<R.Paged<Responses.Rate>> { Payload = paged, Etag = Guid.Empty, SubscriptionId = query.SubscriptionId, SubscriptionTime = query.SubscriptionTime };
            }
        }
    }
}