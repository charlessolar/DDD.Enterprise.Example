using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using NLog;
using NServiceBus.MessageInterfaces;
using NServiceBus.Pipeline;
using RabbitMQ.Client;

namespace Demo.Library.RabbitMq
{
    // Checks incoming messages for RoutingKeyOn attribute, claims a routing key if found
    public class BindRoutedBehavior : Behavior<IIncomingLogicalMessageContext>
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly HashSet<Type> _notRouted = new HashSet<Type>();
        private readonly HashSet<string> _routed = new HashSet<string>();
        private readonly IMessageMapper _mapper;
        private readonly string _instance;

        public BindRoutedBehavior(string instanceQueue, IMessageMapper mapper)
        {
            _instance = instanceQueue;
            _mapper = mapper;
        }

        private PropertyInfo GetRouteOn(Type type)
        {
            var router = (RoutingKeyOnAttribute)Attribute.GetCustomAttribute(type, typeof(RoutingKeyOnAttribute), true);
            if (router == null) return null;

            return type.GetProperty(router.Property,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        }

        public override Task Invoke(IIncomingLogicalMessageContext context, Func<Task> next)
        {
            var type = context.Message.MessageType;
            if(!context.Message.MessageType.IsInterface)
                type = _mapper.GetMappedTypeFor(context.Message.MessageType);

            if (_notRouted.Contains(type))
                return next();

            var routeOn = GetRouteOn(type);
            if (routeOn == null)
            {
                _notRouted.Add(type);
                return next();
            }

            // Got routed message, this is the first one so we are the lucky winner for the key
            // create connection to rabbit
            var rabbit = context.Builder.Build<IConnection>();

            var channel = rabbit.CreateModel();

            // bind InstanceSpecificQueue to header exchange
            var specs = new Dictionary<string, object>
            {
                {"x-match", "all"},
                {"Routing", routeOn.GetValue(context.Message.Instance).ToString()}
            };

            if (_routed.Contains($"{RoutingRoutingTopology.ExchangeName(type)}.routable.{specs["Routing"]}"))
            {
                Logger.Warn($"Received non-routed message {type} when we have routed it!");
                return next();
            }

            _routed.Add(
                $"{RoutingRoutingTopology.ExchangeName(type)}.routable.{specs["Routing"]}");

            Logger.Debug($"Received routable message, binding to routing queue {RoutingRoutingTopology.ExchangeName(type)}.routable");
            channel.QueueBind(_instance, $"{RoutingRoutingTopology.ExchangeName(type)}.routable", string.Empty, specs);

            return next();
        }
    }
}
