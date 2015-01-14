using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Events
{
    public interface AvatarChanged : IEvent
    {
        String UserId { get; set; }

        String ImageType { get; set; }

        String ImageData { get; set; }
    }
}