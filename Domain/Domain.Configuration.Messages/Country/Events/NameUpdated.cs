using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Country.Events
{
    public interface NameUpdated : IEvent
    {
        Guid CountryId { get; set; }

        String Name { get; set; }
    }
}