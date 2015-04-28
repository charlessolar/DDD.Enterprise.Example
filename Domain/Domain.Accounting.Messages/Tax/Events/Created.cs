using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax.Events
{
    public interface Created : IEvent
    {
        Guid TaxId { get; set; }

        String Code { get; set; }

        String Name { get; set; }

        String Description { get; set; }

        Guid TaxTypeId { get; set; }
    }
}