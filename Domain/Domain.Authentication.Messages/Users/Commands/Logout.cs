using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class Logout : ICommand
    {
        public String UserId { get; set; }

        public String Event { get; set; }
    }
}