using NLog;
using NServiceBus;
using System.Threading.Tasks;

namespace Demo.Library.Utility.Handlers
{
    public class UtilityHandler : IHandleMessages<IChangeLogLevel>
    {
        public Task Handle(IChangeLogLevel message, IMessageHandlerContext context)
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
