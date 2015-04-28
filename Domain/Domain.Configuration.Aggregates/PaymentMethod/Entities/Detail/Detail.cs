using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod.Entities.Detail
{
    public class Detail : Aggregates.Entity<Guid>, IDetail
    {
        private Detail()
        {
        }

        public void Create(String Name, String Hint)
        {
            Apply<Events.Created>(e =>
            {
                e.DetailId = Id;
                e.PaymentMethodId = AggregateId;
                e.Name = Name;
                e.Hint = Hint;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.DetailId = Id;
                e.PaymentMethodId = AggregateId;
            });
        }
    }
}