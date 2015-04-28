using Demo.Library.Responses;
using Demo.Library.SSE;
using ServiceStack.Caching;
using System;
using System.Collections.Generic;

namespace Demo.Library.Extensions
{
    public static class SubscriptionExtensions
    {
        //public static void AddTracked<T>(this ISubscriptionManager manager, String CacheKey, Responses.Query<T> Response, String SessionId) where T : IResponse
        //{
        //    manager.AddTracked<T>(Response.SubscriptionId, CacheKey,   Response.Payload.DocumentId(), SessionId, Response.SubscriptionId, Response.SubscriptionTime);

        //    //if (Response.SubscriptionType.HasValue && Response.SubscriptionType.Value.HasFlag(ChangeType.NEW))
        //    //    manager.AddTracked(typeof(T).DocumentId(), SessionId, Response.SubscriptionId, Response.SubscriptionTime);
        //}

        //public static void AddTracked<T>(this ISubscriptionManager manager, Responses.Query<Responses.Paged<T>> Response, String SessionId) where T : IResponse
        //{
        //    foreach (var index in Response.Payload.Records)
        //        manager.AddTracked(index.DocumentId(), SessionId, Response.SubscriptionId, Response.SubscriptionTime);

        //    //if (Response.SubscriptionType.HasValue && Response.SubscriptionType.Value.HasFlag(ChangeType.NEW))
        //    //    manager.AddTracked(typeof(T).DocumentId(), SessionId, Response.SubscriptionId, Response.SubscriptionTime);
        //}

        public static void Publish<T>(this ISubscriptionManager manager, T Payload, ChangeType Type = ChangeType.CHANGE) where T : IResponse
        {
            var docId = Payload.DocumentId();
            //var etag = Guid.Empty;

            manager.Publish(docId, Payload, Type);
        }

        public static void PublishAll<T>(this ISubscriptionManager manager, IEnumerable<T> Payloads, ChangeType Type = ChangeType.CHANGE) where T : IResponse
        {
            foreach (var sub in Payloads)
                Publish(manager, sub, Type);
        }
    }
}