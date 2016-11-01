using NServiceBus.Logging;
using NServiceBus.Pipeline;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Library.Logging
{
    public class LogIncomingMessageBehavior : Behavior<IIncomingLogicalMessageContext>
    {
        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            if ( Log.IsDebugEnabled && context.Message.Instance != null)
            {

                Log.DebugFormat("Received message '{0}'.\n" +
                                "ToString() of the message yields: {1}\n" +
                                "Message headers:\n{2}",
                                context.Message.MessageType != null ? context.Message.MessageType.AssemblyQualifiedName : "unknown",
                    context.Message.Instance,
                    string.Join(", ", context.MessageHeaders.Select(h => h.Key + ":" + h.Value).ToArray()));
            }

            return next();

        }

        static readonly ILog Log = LogManager.GetLogger("LogIncomingMessage");
    }
}
