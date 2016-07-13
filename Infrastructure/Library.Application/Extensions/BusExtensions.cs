using Aggregates.Messages;
using NServiceBus;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using Demo.Library.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Reply;
using Aggregates.Extensions;
using Aggregates;
using System.Configuration;

namespace Demo.Library.Extensions
{
    public static class BusExtensions
    {
        private static String ServiceStackEndpoint = ConfigurationManager.AppSettings["destination.servicestack"];

        public static void ResultAsync<TResponse>(this IHandleContext context, TResponse Payload, String ETag = "") where TResponse : class
        {
            context.ReplyAsync<IReply>(x =>
            {
                x.ETag = ETag;
                x.Payload = Payload;
            });
        }
        public static void ResultAsync<TResponse>(this IHandleContext context, IEnumerable<TResponse> Records, Int64 Total, Int64 ElapsedMs) where TResponse : class
        {
            context.ReplyAsync<IPagedReply>(x =>
            {
                x.Records = Records.ToList();
                x.Total = Total;
                x.ElapsedMs = ElapsedMs;
            });
        }
        public static void UpdateAsync(this IHandleContext context, Object Payload, ChangeType ChangeType, String ETag = "")
        {
            context.SendAsync<Updates.Update>(ServiceStackEndpoint, x =>
            {
                x.Payload = Payload;
                x.ChangeType = ChangeType;
                x.ETag = ETag;
                x.Timestamp = DateTime.UtcNow;
            });
        }
    }
}
