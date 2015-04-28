using Demo.Library.Command;
using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class Logout : DemoCommand
    {

        public String Event { get; set; }
    }
}