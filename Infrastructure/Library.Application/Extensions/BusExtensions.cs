using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using Demo.Library.Reply;
using System.Configuration;
using Demo.Library.SSE;

namespace Demo.Library.Extensions
{
    public static class BusExtensions
    {
        private static string _serviceStackEndpoint = ConfigurationManager.AppSettings["destination.servicestack"];

        public static void Result<TResponse>(this IMessageHandlerContext context, TResponse payload, string eTag = "") where TResponse : class
        {
            context.Reply<IReply>(x =>
            {
                x.ETag = eTag;
                x.Payload = payload;
            });
        }
        public static void Result<TResponse>(this IMessageHandlerContext context, IEnumerable<TResponse> records, long total, long elapsedMs) where TResponse : class
        {
            context.Reply<IPagedReply>(x =>
            {
                x.Records = records.ToList();
                x.Total = total;
                x.ElapsedMs = elapsedMs;
            });
        }
        public static void Update(this IMessageHandlerContext context, object payload, ChangeType changeType, string eTag = "")
        {
            var options = new PublishOptions();
            //options.SetDestination(ServiceStackEndpoint);

            context.Publish<Updates.IUpdate>(x =>
            {
                x.Payload = payload;
                x.ChangeType = changeType;
                x.ETag = eTag;
                x.Timestamp = DateTime.UtcNow;
            }, options);
        }
    }
}
