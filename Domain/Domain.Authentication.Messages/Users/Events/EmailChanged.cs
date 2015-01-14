using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Events
{
    public interface EmailChanged : IEvent
    {
        String UserId { get; set; }

        String Email { get; set; }
    }
}