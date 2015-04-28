using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod.Events
{
    public interface Created : IEvent
    {
        Guid PaymentMethodId { get; set; }

        String Name { get; set; }

        String Description { get; set; }

        Guid? ParentId { get; set; }
    }
}