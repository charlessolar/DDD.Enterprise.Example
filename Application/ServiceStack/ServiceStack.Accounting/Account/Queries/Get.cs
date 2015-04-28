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

namespace Demo.Application.ServiceStack.Accounting.Account.Queries
{
    public class Get : IQueryHandler<Services.Get, Responses.Get>
    {
        private readonly IDocumentStore _store;

        public Get(IDocumentStore store)
        {
            _store = store;
        }

        public bool Satisfied(Services.Get query, Responses.Get dto)
        {
            throw new NotImplementedException();
        }

        public R.Query<Responses.Get> Handle(Services.Get query)
        {
            using (var session = _store.OpenSession())
            {
                var dto = session.Load<Responses.Get>(query.AccountId);

                return new R.Query<Responses.Get> { Payload = dto, Etag = Guid.Empty, SubscriptionId = query.SubscriptionId, SubscriptionTime = query.SubscriptionTime };
            }
        }
    }
}