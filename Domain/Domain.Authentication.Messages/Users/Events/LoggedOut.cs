using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Events
{
    public interface LoggedOut : IEvent
    {
        String UserId { get; set; }
    }
}