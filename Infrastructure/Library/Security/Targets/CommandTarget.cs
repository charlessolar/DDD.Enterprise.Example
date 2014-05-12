using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Targets
{
    public class CommandTarget<TCommand> : Target where TCommand : ICommand
    {
        public CommandTarget()
            : base("COMMAND")
        {
        }
    }
}