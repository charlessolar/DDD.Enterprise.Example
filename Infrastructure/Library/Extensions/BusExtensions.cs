using System;
using System.Threading.Tasks;
using NServiceBus;

namespace Demo.Library.Extensions
{
    public static class BusExtensions
    {

        public static Task SendToSelf<TEvent>(this IMessageSession bus, Action<TEvent> @event)
        {
            var options = new SendOptions();
            options.RouteToThisInstance();
            return bus.Send<TEvent>(@event, options);
        }
    }
}
