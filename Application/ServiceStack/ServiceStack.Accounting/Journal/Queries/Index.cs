using Demo.Library.Extensions;
using Demo.Library.Queries.Processor;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Q = Demo.Library.Queries;
using R = Demo.Library.Responses;

namespace Demo.Application.ServiceStack.Accounting.Journal.Queries
{
    public class Index : IPagingQueryHandler<Services.Index, Responses.Index>
    {
        private readonly Nest.IElasticClient _client;

        public Index(Nest.IElasticClient client)
        {
            _client = client;
        }

        public bool Satisfied(Services.Index query, Responses.Index dto)
        {
            throw new NotImplementedException();
        }

        public R.Query<R.Paged<Responses.Index>> Handle(Services.Index query)
        {
            var results = _client.Search<Responses.Index>(s => s.Paging(query));

            var paged = new R.Paged<Responses.Index> { Records = results.Documents, Total = results.Total, ElapsedMs = results.ElapsedMilliseconds };

            return new R.Query<R.Paged<Responses.Index>> { Payload = paged, Etag = Guid.Empty, SubscriptionId = query.SubscriptionId, SubscriptionTime = query.SubscriptionTime };
        }
    }
}