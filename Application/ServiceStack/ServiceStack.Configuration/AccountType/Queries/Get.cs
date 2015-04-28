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

namespace Demo.Application.ServiceStack.Configuration.AccountType.Queries
{
    public class Get : IQueryHandler<Services.Get, Responses.Index>
    {
        private readonly IDocumentStore _store;

        public Get(IDocumentStore store)
        {
            _store = store;
        }

        public bool Satisfied(Services.Get query, Responses.Index dto)
        {
            throw new NotImplementedException();
        }

        public R.Query<Responses.Index> Handle(Services.Get query)
        {
            using (var session = _store.OpenSession())
            {
                var dto = session.Load<Responses.Index>(query.AccountTypeId);

                var response = new R.Query<Responses.Index> { Payload = dto, Etag = Guid.Empty, SubscriptionId = query.SubscriptionId, SubscriptionTime = query.SubscriptionTime };

                if (query.SubscriptionId.IsNullOrEmpty())
                    return response;

                return response;
            }
        }
    }
}