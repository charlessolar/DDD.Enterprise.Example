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

namespace Demo.Application.ServiceStack.Accounting.Account.Queries
{
    public class Select : IPagingQueryHandler<Services.Select, Responses.Index>
    {
        private readonly Nest.IElasticClient _client;

        public Select(Nest.IElasticClient client)
        {
            _client = client;
        }

        public bool Satisfied(Services.Select query, Responses.Index dto)
        {
            throw new NotImplementedException();
        }

        public R.Query<R.Paged<Responses.Index>> Handle(Services.Select query)
        {
            if (query.Id.HasValue)
            {
                var result = _client.Get<Responses.Index>(query.Id.Value);
                var paged = new R.Paged<Responses.Index> { Records = new[] { result.Source }, Total = 1 };

                return new R.Query<R.Paged<Responses.Index>> { Payload = paged };
            }
            else
            {
                var results = _client.Search<Responses.Index>(s => s
                    .Take(Int32.MaxValue)
                    .Query(q => q.Term(f => f.Code, query.Term, Boost: 2) || q.Term(f => f.ParentCode, query.Term, Boost: 1) || q.Term(f => f.Name, query.Term, 5))
                    );
                //.Query(q => q.QueryString(qs => qs.OnFieldsWithBoost(d => d.Add(e => e.ParentCode, 2).Add(e => e.Code, 1).Add(e => e.Name, 5)).WildQuery(request.Term))));

                var paged = new R.Paged<Responses.Index> { Records = results.Documents, Total = results.Total, ElapsedMs = results.ElapsedMilliseconds };
                return new R.Query<R.Paged<Responses.Index>> { Payload = paged, Etag = Guid.Empty, SubscriptionId = query.SubscriptionId, SubscriptionTime = query.SubscriptionTime };
            }
        }
    }
}