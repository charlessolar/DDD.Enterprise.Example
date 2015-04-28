using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod.Entities.Detail.Events
{
    public interface Created : IEvent
    {
        Guid PaymentMethodId { get; set; }

        Guid DetailId { get; set; }

        String Name { get; set; }

        String Hint { get; set; }
    }
}