using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using NLog;
using NServiceBus;
using NServiceBus.MessageInterfaces;
using NServiceBus.Transport;
using NServiceBus.Transport.RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Demo.Library.RabbitMq
{
    // Examines types being sent to RabbitMq for [RoutingKeyOn("X")] attributes, create seperate exchanges and single bound queue for different values found in property
    public class RoutingRoutingTopology : IRoutingTopology
    {
        private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly HashSet<Type> _notRouted = new HashSet<Type>();
        private readonly HashSet<Type> _isRouted = new HashSet<Type>();
        private readonly ConcurrentDictionary<Type, string> _typeTopologyConfiguredSet = new ConcurrentDictionary<Type, string>();
        private readonly ConcurrentDictionary<string, TaskCompletionSource<bool>> _routableConfirms = new ConcurrentDictionary<string, TaskCompletionSource<bool>>();
        private readonly ConcurrentDictionary<ulong, string> _tagToType = new ConcurrentDictionary<ulong, string>();

        public IMessageMapper Mapper { get; set; }

        public void Initialize(IModel channel, string mainQueue)
        {
            CreateExchange(channel, mainQueue);
            channel.QueueBind(mainQueue, mainQueue, string.Empty);
        }


        public void Publish(IModel channel, Type type, OutgoingMessage message, IBasicProperties properties)
        {
            if (!type.IsInterface)
                type = Mapper?.GetMappedTypeFor(type) ?? type;
            
            SetupTypeSubscriptions(channel, type);

            // Explanation:
            // A routable message has an attribute "RoutingKeyOn" which specifies a value of the message body which
            // can be used to delegate processing to a single consumer
            // In this topology we are creating Headers exchanges for types that have this attribute
            // when we are publishing the message we'll try to send it to the routable exchange first
            // that exchange contains bindings for consumers who are matching a specific header.
            // If the message is returned, that means no consumer is listening for that header,
            // so then we publish to the main exchange, and some consumer will pick it up and bind itself 
            // to the headers exchange.

            EventHandler<BasicReturnEventArgs> retHandler = (sender, e) =>
            {
                Logger.Debug($"Received returned message from {e.Exchange}");

                
                _tagToType.TryAdd(channel.NextPublishSeqNo, e.Exchange);
                
                channel.BasicPublish(e.Exchange.Substring(0, e.Exchange.Length - 9), string.Empty, false, e.BasicProperties, e.Body);
            };
            EventHandler<BasicAckEventArgs> ackHandler = (sender, e) =>
            {
                string t;
                if (!_tagToType.TryRemove(e.DeliveryTag, out t))
                    return;

                TaskCompletionSource<bool> existing;
                if (_routableConfirms.TryRemove(t, out existing))
                    existing.SetResult(true);
            };

            if (_notRouted.Contains(type))
            {
                channel.BasicPublish(ExchangeName(type), string.Empty, false, properties, message.Body);
                return;
            }
            if (_isRouted.Contains(type))
            {
                Logger.Debug($"Publishing routed message {type.FullName}");
                channel.BasicReturn -= retHandler;
                channel.BasicReturn += retHandler;
                channel.BasicAcks -= ackHandler;
                channel.BasicAcks += ackHandler;

                TaskCompletionSource<bool> existing;
                if (_routableConfirms.TryGetValue(ExchangeName(type), out existing))
                    existing.Task.Wait();
                _tagToType.TryAdd(channel.NextPublishSeqNo, ExchangeName(type));

                var task = GetRoutableTask(type);
                channel.BasicPublish($"{ExchangeName(type)}.routable", string.Empty, true, properties, message.Body);
                task.Wait();
                return;

            }


            if (!IsTypeRoutable(type))
                _notRouted.Add(type);
            else
            {
                Logger.Debug($"Found routed message type {type.FullName}");
                _isRouted.Add(type);
            }
            Publish(channel, type, message, properties);
        }

        private Task GetRoutableTask(Type type)
        {
            var tcs = new TaskCompletionSource<bool>();
            var added = _routableConfirms.TryAdd(ExchangeName(type), tcs);
            if (!added)
            {
                throw new Exception($"Failed to add task for type {type}");
            }
            return tcs.Task;
        }

        public void RawSendInCaseOfFailure(IModel channel, string address, byte[] body, IBasicProperties properties)
        {
            channel.BasicPublish(address, string.Empty, true, properties, body);
        }

        public void Send(IModel channel, string address, OutgoingMessage message, IBasicProperties properties)
        {
            channel.BasicPublish(address, string.Empty, true, properties, message.Body);
        }

        public void SetupSubscription(IModel channel, Type type, string subscriberName)
        {
            if(!type.IsInterface)
                type = Mapper?.GetMappedTypeFor(type) ?? type;

            if (type == typeof(IEvent))
            {
                // Make handlers for IEvent handle all events whether they extend IEvent or not
                type = typeof(object);
            }

            SetupTypeSubscriptions(channel, type);
            channel.ExchangeBind(subscriberName, ExchangeName(type), string.Empty);
        }

        public void TeardownSubscription(IModel channel, Type type, string subscriberName)
        {
            if (!type.IsInterface)
                type = Mapper?.GetMappedTypeFor(type) ?? type;

            try
            {
                channel.ExchangeUnbind(subscriberName, ExchangeName(type), string.Empty, null);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause
            {
                // TODO: Any better way to make this idempotent?
            }
        }
        public static string ExchangeName(Type type) => type.Namespace + ":" + type.Name;

        void SetupTypeSubscriptions(IModel channel, Type type)
        {
            if (!type.IsInterface)
                type = Mapper?.GetMappedTypeFor(type) ?? type;

            if (type == typeof(object) || IsTypeTopologyKnownConfigured(type))
            {
                return;
            }

            var typeToProcess = type;
            CreateExchange(channel, ExchangeName(typeToProcess), IsTypeRoutable(typeToProcess));
            var baseType = typeToProcess.BaseType;

            while (baseType != null)
            {
                CreateExchange(channel, ExchangeName(baseType));

                channel.ExchangeBind(ExchangeName(baseType), ExchangeName(typeToProcess), string.Empty);
                typeToProcess = baseType;
                baseType = typeToProcess.BaseType;
            }

            foreach (var interfaceType in type.GetInterfaces())
            {
                var exchangeName = ExchangeName(interfaceType);

                CreateExchange(channel, exchangeName);
                channel.ExchangeBind(exchangeName, ExchangeName(type), string.Empty);
            }

            MarkTypeConfigured(type);
        }

        bool IsTypeRoutable(Type eventType)
            => (RoutingKeyOnAttribute)Attribute.GetCustomAttribute(eventType, typeof(RoutingKeyOnAttribute), true) != null;

        void MarkTypeConfigured(Type eventType)
        {
            _typeTopologyConfiguredSet[eventType] = null;
        }

        bool IsTypeTopologyKnownConfigured(Type eventType) => _typeTopologyConfiguredSet.ContainsKey(eventType);

        void CreateExchange(IModel channel, string exchangeName, bool headers = false)
        {
            try
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true, true);
                if (headers)
                    channel.ExchangeDeclare($"{exchangeName}.routable", ExchangeType.Headers, true, true);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause
            {
                // TODO: Any better way to make this idempotent?
            }
        }

    }
}
