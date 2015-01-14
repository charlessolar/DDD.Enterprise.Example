using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Events
{
    public interface LoggedIn : IEvent
    {
        String UserId { get; set; }

        String Name { get; set; }

        String Email { get; set; }

        String NickName { get; set; }

        String ImageType { get; set; }

        String ImageData { get; set; }
    }
}