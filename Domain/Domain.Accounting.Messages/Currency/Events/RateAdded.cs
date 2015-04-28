using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Events
{
    public interface RateAdded : IEvent
    {
        Guid RateId { get; set; }
        Guid CurrencyId { get; set; }
        Guid DestinationCurrencyId { get; set; }

        Decimal Factor { get; set; }

        
        DateTime? EffectiveTill { get; set; }
    }
}