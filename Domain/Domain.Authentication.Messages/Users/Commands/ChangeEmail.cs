using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class ChangeEmail : ICommand
    {
        public String UserId { get; set; }

        public String Email { get; set; }
    }
}