using System;
using System.Collections.Concurrent;
using System.Configuration;
using NLog;
using NServiceBus;
using NServiceBus.Transport;
using NServiceBus.Transport.RabbitMQ;
using RabbitMQ.Client;

namespace Demo.Library.RabbitMq
{
    public class ShardedRoutingTopology : IRoutingTopology
    {
        private static NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        public void SetupSubscription(IModel channel, Type type, string subscriberName)
        {
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

        public void Publish(IModel channel, Type type, OutgoingMessage message, IBasicProperties properties)
        {
            SetupTypeSubscriptions(channel, type);
            channel.BasicPublish(ExchangeName(type), message.MessageId, false, properties, message.Body);
        }

        public void Send(IModel channel, string address, OutgoingMessage message, IBasicProperties properties)
        {
            channel.BasicPublish(address, message.MessageId, true, properties, message.Body);
        }

        public void RawSendInCaseOfFailure(IModel channel, string address, byte[] body, IBasicProperties properties)
        {
            channel.BasicPublish(address, string.Empty, true, properties, body);
        }

        public void Initialize(IModel channel, string mainQueue)
        {
            if (mainQueue == ConfigurationManager.AppSettings["endpoint"])
            {
                channel.ExchangeDeclare(mainQueue, "x-modulus-hash", true);
            }
            else
            {
                channel.QueueDeclare(mainQueue, true, false, false, null);
                CreateExchange(channel, mainQueue);
                channel.QueueBind(mainQueue, mainQueue, string.Empty);
            }
        }

        static string ExchangeName(Type type) => type.Namespace + ":" + type.Name;

        void SetupTypeSubscriptions(IModel channel, Type type)
        {
            if (type == typeof(object) || IsTypeTopologyKnownConfigured(type))
            {
                return;
            }

            var typeToProcess = type;
            CreateExchange(channel, ExchangeName(typeToProcess));
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

        void MarkTypeConfigured(Type eventType)
        {
            _typeTopologyConfiguredSet[eventType] = null;
        }

        bool IsTypeTopologyKnownConfigured(Type eventType) => _typeTopologyConfiguredSet.ContainsKey(eventType);

        void CreateExchange(IModel channel, string exchangeName)
        {
            try
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout, true);
            }
            // ReSharper disable EmptyGeneralCatchClause
            catch (Exception)
            // ReSharper restore EmptyGeneralCatchClause
            {
                // TODO: Any better way to make this idempotent?
            }
        }

        readonly ConcurrentDictionary<Type, string> _typeTopologyConfiguredSet = new ConcurrentDictionary<Type, string>();

    }
}
