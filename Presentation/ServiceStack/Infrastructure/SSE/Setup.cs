using Demo.Library.Setup;
using Demo.Library.Setup.Attributes;
using Demo.Library.SSE;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Infrastructure.SSE
{
    [Operation("SSE")]
    [Category("SQL")]
    public class Setup : ISetup
    {
        private readonly IDbConnectionFactory _sql;
        public Setup(IDbConnectionFactory sql)
        {
            _sql = sql;
        }

        public Boolean Initialize()
        {
            using (var db = _sql.Open())
            {
                typeof(Subscription).AddAttributes(new AliasAttribute("SSE.Subscriptions"));

                if(db.CreateTableIfNotExists<Subscription>())
                {
                    db.CreateIndex<Subscription>(x => x.SubscriptionId);
                    db.CreateIndex<Subscription>(x => x.Document);
                    db.CreateIndex<Subscription>(x => x.DocumentId);
                    db.CreateIndex<Subscription>(x => x.CacheKey);

                    db.CreateIndex<Subscription>(x => x.Expires);

                }
            }
            this.Done = true;
            return true;
        }

        public Boolean Done { get; private set; }
    }
}
