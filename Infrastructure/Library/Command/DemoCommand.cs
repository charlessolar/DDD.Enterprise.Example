using FluentValidation.Internal;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Command
{
    public class DemoCommand : ICommand
    {
        public Int64 Timestamp { get; set; }

        public String UserId { get; set; }
    }
}