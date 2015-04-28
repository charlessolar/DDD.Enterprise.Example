using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Region.Events
{
    public interface NameChanged : IEvent
    {
        Guid RegionId { get; set; }

        String Name { get; set; }
    }
}