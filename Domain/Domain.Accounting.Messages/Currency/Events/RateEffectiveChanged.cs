using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Events
{
    public interface RateEffectiveChanged : IEvent
    {
        Guid CurrencyId { get; set; }
        Guid RateId { get; set; }
        DateTime? EffectiveTill { get; set; }
    }
}
