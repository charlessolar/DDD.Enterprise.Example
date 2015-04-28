using Demo.Library.Command;
using Demo.Library.Exceptions;
using NServiceBus;
using ServiceStack;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
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

        public static Task<TResponse> IsCommand<TResponse>(this ICallback callback)
        {
            return callback.Register(x =>
            {
                var reply = x.Messages.FirstOrDefault();
                if (reply == null || reply is Reject)
                {
                    var reject = reply as Reject;
                    if (reject != null)
                        throw new CommandRejectedException(reject.Message);
                    throw new CommandRejectedException();
                }
                return reply.ConvertTo<TResponse>();
            });
        }
    }
}