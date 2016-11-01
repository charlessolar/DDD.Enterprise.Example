using Aggregates.Extensions;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace Demo.Domain.Infrastructure.Extensions
{
    public static class BusExtensions
    {

        public static async Task DomainCommand(this IMessageSession ctx, ICommand command)
        {
            var options = new NServiceBus.SendOptions();
            options.RouteToThisEndpoint();
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await ctx.Request<IMessage>(command, options).ConfigureAwait(false);
            response.CommandResponse();
        }
        public static async Task DomainCommand<TCommand>(this IMessageSession ctx, Action<TCommand> command) where TCommand : ICommand
        {
            var options = new NServiceBus.SendOptions();
            options.RouteToThisEndpoint();
            options.SetHeader(Aggregates.Defaults.RequestResponse, "1");

            var response = await ctx.Request<IMessage>(command, options).ConfigureAwait(false);
            response.CommandResponse();
        }
    }
}
