using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax.Events
{
    public interface Activated : IEvent
    {
        Guid TaxId { get; set; }

        String UserId { get; set; }
    }
}