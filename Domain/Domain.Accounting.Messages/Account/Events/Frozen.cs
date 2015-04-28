using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Events
{
    public interface Frozen : IEvent
    {
        Guid AccountId { get; set; }
        Boolean Frozen { get; set; }
    }
}
