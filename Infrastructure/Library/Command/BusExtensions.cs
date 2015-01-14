using NServiceBus;
using System;

namespace Demo.Library.Command
{
    public static class BusExtensions
    {
        public static void Accept(this IBus bus)
        {
            bus.Reply<Accept>(e => { });
        }

        public static void Reject(this IBus bus, String Message)
        {
            bus.Reply<Reject>(e => { e.Message = Message; });
        }
    }
}