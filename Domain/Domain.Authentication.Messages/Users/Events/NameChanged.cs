using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Events
{
    public interface NameChanged : IEvent
    {
        String UserId { get; set; }

        String Name { get; set; }
    }
}