using NServiceBus;
using System;
using System.Threading.Tasks;
using System.Configuration;
using Demo.Library.Queries;
using Demo.Library.Command;
using Aggregates.Extensions;
using Q = Demo.Presentation.ServiceStack.Infrastructure.Queries;
using R = Demo.Presentation.ServiceStack.Infrastructure.Responses;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class BusExtensions
    {
        private static readonly string RiakEndpoint = ConfigurationManager.AppSettings["destination.riak"];
        private static readonly string ElasticEndpoint = ConfigurationManager.AppSettings["destination.elastic"];
        private static readonly string DomainEndpoint = ConfigurationManager.AppSettings["destination.domain"];

        public static async Task<R.ResponsesQuery<TResponse>> RequestToRiak<T, TResponse>(this IMessageSession bus, Action<T> messageConstructor, Q.QueriesQuery<TResponse> query) where T : IQuery where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination(RiakEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(messageConstructor, options).ConfigureAwait(false);
            return response.RequestQuery(query);
        }
        public static async Task<R.ResponsesPaged<TResponse>> RequestToRiak<T, TResponse>(this IMessageSession bus, Action<T> messageConstructor, Q.QueriesPaged<TResponse> query) where T : IPaged where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination(RiakEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(messageConstructor, options).ConfigureAwait(false);
            return response.RequestPaged(query);
        }
        public static async Task<R.ResponsesQuery<TResponse>> RequestToRiak<T, TResponse>(this IMessageSession bus, T message, Q.QueriesQuery<TResponse> query) where T : IQuery where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination(RiakEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(message, options).ConfigureAwait(false);
            return response.RequestQuery(query);
        }
        public static async Task<R.ResponsesPaged<TResponse>> RequestToRiak<T, TResponse>(this IMessageSession bus, T message, Q.QueriesPaged<TResponse> query) where T : IPaged where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination(RiakEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(message, options).ConfigureAwait(false);
            return response.RequestPaged(query);
        }



        public static async Task<R.ResponsesQuery<TResponse>> RequestToElastic<T, TResponse>(this IMessageSession bus, Action<T> messageConstructor, Q.QueriesQuery<TResponse> query) where T : IQuery where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination(ElasticEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(messageConstructor, options).ConfigureAwait(false);
            return response.RequestQuery(query);
        }
        public static async Task<R.ResponsesPaged<TResponse>> RequestToElastic<T, TResponse>(this IMessageSession bus, Action<T> messageConstructor, Q.QueriesPaged<TResponse> query) where T : IPaged where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination(ElasticEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(messageConstructor, options).ConfigureAwait(false);
            return response.RequestPaged(query);
        }
        public static async Task<R.ResponsesQuery<TResponse>> RequestToElastic<T, TResponse>(this IMessageSession bus, T message, Q.QueriesQuery<TResponse> query) where T : IQuery where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination(RiakEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(message, options).ConfigureAwait(false);
            return response.RequestQuery(query);
        }
        public static async Task<R.ResponsesPaged<TResponse>> RequestToElastic<T, TResponse>(this IMessageSession bus, T message, Q.QueriesPaged<TResponse> query) where T : IPaged where TResponse : class
        {
            var options = new SendOptions();
            options.SetDestination(ElasticEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(message, options).ConfigureAwait(false);
            return response.RequestPaged(query);
        }



        public static async Task CommandToDomain<T>(this IMessageSession bus, Action<T> messageConstructor) where T : DemoCommand
        {
            var options = new SendOptions();
            options.SetDestination(DomainEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(messageConstructor, options).ConfigureAwait(false);
            response.CommandResponse();
        }
        public static async Task CommandToDomain<T>(this IMessageSession bus, T message) where T : DemoCommand
        {
            var options = new SendOptions();
            options.SetDestination(DomainEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await bus.Request<IMessage>(message, options).ConfigureAwait(false);
            response.CommandResponse();
        }

        public static Task SendToDomain<T>(this IMessageSession bus, T message)
        {
            var options = new SendOptions();
            options.SetDestination(DomainEndpoint);
            options.SetHeader(Aggregates.Defaults.RequestResponse, "0");

            return bus.Send(message, options);
        }
    }
}
