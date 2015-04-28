using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType.Events
{
    public interface DeferralChanged : IEvent
    {
        Guid AccountTypeId { get; set; }

        String DeferralMethod { get; set; }
    }
}