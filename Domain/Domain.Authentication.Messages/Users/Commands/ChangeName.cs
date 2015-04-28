using Demo.Library.Command;
using NServiceBus;
using System;

namespace Demo.Domain.Authentication.Users.Commands
{
    public class ChangeName : DemoCommand
    {
        public String Name { get; set; }
    }
}