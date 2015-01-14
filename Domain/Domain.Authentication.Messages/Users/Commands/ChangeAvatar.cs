using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class ChangeAvatar : ICommand
    {
        public String UserId { get; set; }

        public String ImageType { get; set; }

        public String ImageData { get; set; }
    }
}