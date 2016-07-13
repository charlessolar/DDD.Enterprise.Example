using Aggregates.Messages;
using NServiceBus;
using Demo.Library.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Demo.Library.Queries;
using Demo.Library.Command;
using Aggregates.Extensions;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class BusExtensions
    {
        private static String RiakEndpoint = ConfigurationManager.AppSettings["destination.riak"];
        private static String RavenEndpoint = ConfigurationManager.AppSettings["destination.raven"];
        private static String ElasticEndpoint = ConfigurationManager.AppSettings["destination.elastic"];
        private static String DomainEndpoint = ConfigurationManager.AppSettings["destination.domain"];
        public static ICallback SendToRiak<T>(this IBus bus, Action<T> messageConstructor) where T : IQuery
        {
            return bus.Send(RiakEndpoint, messageConstructor);
        }
        public static ICallback SendToRiak<T>(this IBus bus, T query) where T : IQuery
        {
            return bus.Send(RiakEndpoint, query);
        }
        public static ICallback SendToRaven<T>(this IBus bus, Action<T> messageConstructor) where T : IQuery
        {
            return bus.Send(RavenEndpoint, messageConstructor);
        }
        public static ICallback SendToRaven<T>(this IBus bus, T query) where T : IQuery
        {
            return bus.Send(RavenEndpoint, query);
        }
        public static ICallback SendToElastic<T>(this IBus bus, Action<T> messageConstructor) where T : IQuery
        {
            return bus.Send(ElasticEndpoint, messageConstructor);
        }
        public static ICallback SendToElastic<T>(this IBus bus, T query) where T : IQuery
        {
            return bus.Send(ElasticEndpoint, query);
        }
        public static Task CommandToDomain<T>(this IBus bus, Action<T> messageConstructor) where T : DemoCommand
        {
            return bus.Send(DomainEndpoint, messageConstructor).AsCommandResult();
        }
        public static ICallback SendToDomain<T>(this IBus bus, T message)
        {
            return bus.Send(DomainEndpoint, message);
        }
        public static Task CommandToDomain<T>(this IBus bus, T message) where T : DemoCommand
        {
            return bus.Command(DomainEndpoint, message);
        }
    }
}
