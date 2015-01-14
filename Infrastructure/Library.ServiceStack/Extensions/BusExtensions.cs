using Demo.Library.Command;
using Demo.Library.Exceptions;
using NServiceBus;
using ServiceStack;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Library.Extensions
{
    public static class BusExtensions
    {
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