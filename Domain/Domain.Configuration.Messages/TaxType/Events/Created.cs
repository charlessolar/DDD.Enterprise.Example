using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.TaxType.Events
{
    public interface Created : IEvent
    {
        Guid TaxTypeId { get; set; }

        String Name { get; set; }
    }
}