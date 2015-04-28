using Demo.Library.Command;
using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class ChangeEmail : DemoCommand
    {
        public String Email { get; set; }
    }
}