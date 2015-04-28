using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Authentication.Users.Events
{
    public interface TimezoneChanged : IEvent
    {
        String UserId { get; set; }

        String Timezone { get; set; }
    }
}