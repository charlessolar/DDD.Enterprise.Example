using NServiceBus.Logging;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Logging
{
    public class LogIncomingMessageBehavior : IBehavior<IncomingContext>
    {
        public void Invoke(IncomingContext context, Action next)
        {
            if ( log.IsDebugEnabled && context.IncomingLogicalMessage != null)
            {

                log.DebugFormat("Received message '{0}'.\n" +
                                "ToString() of the message yields: {1}\n" +
                                "Message headers:\n{2}",
                                context.IncomingLogicalMessage.MessageType != null ? context.IncomingLogicalMessage.MessageType.AssemblyQualifiedName : "unknown",
                    context.IncomingLogicalMessage.Instance,
                    string.Join(", ", context.IncomingLogicalMessage.Headers.Select(h => h.Key + ":" + h.Value).ToArray()));
            }

            next();

        }

        static ILog log = LogManager.GetLogger("LogIncomingMessage");
    }
    public class LogIncomingRegistration : RegisterStep
    {
        public LogIncomingRegistration()
            : base("LogIncoming", typeof(LogIncomingMessageBehavior), "Logs incoming messages")
        {
            InsertBefore(WellKnownStep.LoadHandlers);
        }
    }
}
