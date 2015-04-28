using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Events
{
    public interface DescriptionChanged : IEvent
    {
        Guid AccountId { get; set; }
        String Description { get; set; }
    }
}
