using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod
{
    public class PaymentMethod : Aggregates.Aggregate<Guid>, IPaymentMethod
    {
        private PaymentMethod()
        {
        }

        public void Create(String Name, String Description, Guid? ParentId)
        {
            Apply<Events.Created>(e =>
            {
                e.PaymentMethodId = Id;
                e.Name = Name;
                e.Description = Description;
                e.ParentId = ParentId;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.PaymentMethodId = Id;
            });
        }
    }
}