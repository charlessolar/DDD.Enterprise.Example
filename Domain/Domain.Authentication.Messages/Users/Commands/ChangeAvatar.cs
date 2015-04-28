using Demo.Library.Command;
using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class ChangeAvatar : DemoCommand
    {
        public String ImageType { get; set; }

        public String ImageData { get; set; }
    }
}