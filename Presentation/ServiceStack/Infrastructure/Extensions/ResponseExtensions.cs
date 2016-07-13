
using NServiceBus;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using Demo.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Q = Demo.Presentation.ServiceStack.Infrastructure.Queries;
using R = Demo.Presentation.ServiceStack.Infrastructure.Responses;
using ServiceStack.Logging;
using Demo.Library.Queries;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class ResponseExtensions
    {
        private static ILog Logger = LogManager.GetLogger("Responses");
        public static String DocumentId<T>(this T dto)
        {
            var id = typeof(T).GetProperty("Id");

            if (id == null)
                throw new ArgumentException($"Type {dto.GetType().FullName} does not have an 'Id' property");

            return id.GetValue(dto).ToString();
        }

        public static R.Responses_Query<TResponse> AsSyncQueryResult<TResponse>(this ICallback callback, Q.Queries_Query<TResponse> query = null) where TResponse :class
        {
            return Task.Run(async () =>
            {
                return await callback.AsQueryResult<TResponse>();
            }).Result;
        }
        public static R.Responses_Paged<TResponse> AsSyncPagedResult<TResponse>(this ICallback callback, Q.Queries_Paged<TResponse> query = null) where TResponse : class
        {
            return Task.Run(async () =>
            {
                return await callback.AsPagedResult<TResponse>();
            }).Result;
        }

        public async static Task<R.Responses_Query<TResponse>> AsQueryResult<TResponse>(this ICallback callback, Q.Queries_Query<TResponse> query = null) where TResponse : class
        {
            var response = await callback.IsQuery<TResponse>();

            return new R.Responses_Query<TResponse>
            {
                Payload = response.Payload.ConvertTo<TResponse>(),
                Etag = response.ETag,
                SubscriptionId = query?.SubscriptionId,
                SubscriptionTime = query?.SubscriptionTime,
            };
        }
        public async static Task<R.Responses_Paged<TResponse>> AsPagedResult<TResponse>(this ICallback callback, Q.Queries_Paged<TResponse> query = null) where TResponse : class
        {
            var response = await callback.IsPaged<TResponse>();
            
            return new R.Responses_Paged<TResponse>
            {
                Records = response.Records.Select(x => x.ConvertTo<TResponse>()),
                Total = response.Total,
                ElapsedMs = response.ElapsedMs,
                SubscriptionId = query?.SubscriptionId,
                SubscriptionTime = query?.SubscriptionTime,
            };

        }

        public static void SubscribeWith<TResponse>(this ISubscriptionManager manager, R.Responses_Paged<TResponse> dto, Q.Queries_Paged<TResponse> query, String Session)
        {
            var key = query.GetCacheKey();
            query.SubscriptionType = query.SubscriptionType ?? ChangeType.ALL;
            foreach (var record in dto.Records)
            {
                manager.Manage(record, key, query.SubscriptionId, query.SubscriptionType ?? ChangeType.ALL, TimeSpan.FromSeconds(query.SubscriptionTime ?? 3600), Session);
            }

        }
        public static void SubscribeWith<TResponse>(this ISubscriptionManager manager, R.Responses_Query<TResponse> dto, Q.Queries_Query<TResponse> query, String Session)
        {
            var key = query.GetCacheKey();
            manager.Manage(dto.Payload, key, query.SubscriptionId, query.SubscriptionType ?? ChangeType.ALL, TimeSpan.FromSeconds(query.SubscriptionTime ?? 3600), Session);

        }
    }
}