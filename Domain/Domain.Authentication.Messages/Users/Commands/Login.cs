using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class Login : ICommand
    {
        public String UserId { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String NickName { get; set; }

        public String ImageType { get; set; }

        public String ImageData { get; set; }
    }
}