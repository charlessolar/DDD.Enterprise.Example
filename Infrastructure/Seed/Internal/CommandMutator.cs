using Aggregates.Contracts;
using NServiceBus;
using System;
using System.Collections.Generic;
using Demo.Library.Command;
using Demo.Library.Extensions;

namespace Seed.Internal
{
    public class CommandMutator : ICommandMutator
    {
        public ICommand MutateIncoming(ICommand command, IReadOnlyDictionary<string, string> headers)
        {
            return command;
        }
        public ICommand MutateOutgoing(ICommand command)
        {
            if (!(command is DemoCommand)) return command;

            (command as DemoCommand).Stamp = DateTime.UtcNow.ToUnix();

            return command;
        }
    }
}
