using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Events
{
    public interface Deactivated : IEvent
    {
        Guid CurrencyId { get; set; }

        String UserId { get; set; }
    }
}