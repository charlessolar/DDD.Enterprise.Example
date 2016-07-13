using Aggregates;
using NLog;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Utility.Handlers
{
    public class UtilityHandler : IHandleMessagesAsync<ChangeLogLevel>
    {
        public Task Handle(ChangeLogLevel message, IHandleContext context)
        {
            foreach( var rule in LogManager.Configuration.LoggingRules)
            {
                rule.EnableLoggingForLevel(message.Level);
            }
            LogManager.ReconfigExistingLoggers();

            return Task.FromResult(0);
        }
    }
}
