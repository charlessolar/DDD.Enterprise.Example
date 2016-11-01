using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NLog;
using NServiceBus.MessageInterfaces;
using NServiceBus.Pipeline;

namespace Demo.Library.RabbitMq
{
    public class OutgoingRoutingKeyHeader : Behavior<IOutgoingLogicalMessageContext>
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly HashSet<Type> _notRouted = new HashSet<Type>();
        private readonly IMessageMapper _mapper;

        public OutgoingRoutingKeyHeader(IMessageMapper mapper)
        {
            _mapper = mapper;
        }

        private PropertyInfo GetRouteOn(Type type)
        {
            var router = (RoutingKeyOnAttribute)Attribute.GetCustomAttribute(type, typeof(RoutingKeyOnAttribute), true);
            if (router == null) return null;

            return type.GetProperty(router.Property,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }

        public override Task Invoke(IOutgoingLogicalMessageContext context, Func<Task> next)
        {
            var type = _mapper.GetMappedTypeFor(context.Message.MessageType);

            if (_notRouted.Contains(type))
                return next();

            var routeOn = GetRouteOn(type);
            if (routeOn == null)
            {
                _notRouted.Add(type);
                return next();
            }

            Logger.Debug($"Outgoing message {type.FullName} is routable, adding routing header");
            // Add route to headers
            context.Headers["Routing"] = routeOn.GetValue(context.Message.Instance).ToString();
            
            return next();
        }
    }
}
