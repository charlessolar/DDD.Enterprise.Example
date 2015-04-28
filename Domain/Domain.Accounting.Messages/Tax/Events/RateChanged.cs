using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax.Events
{
    public interface RateChanged : IEvent
    {
        Guid TaxId { get; set; }

        Boolean Fixed { get; set; }

        Decimal Rate { get; set; }
    }
}