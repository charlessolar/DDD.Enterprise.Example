using Demo.Library.SSE;
using System;
using Q = Demo.Presentation.ServiceStack.Infrastructure.Queries;
using R = Demo.Presentation.ServiceStack.Infrastructure.Responses;
using ServiceStack.Logging;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class ResponseExtensions
    {
        private static ILog _logger = LogManager.GetLogger("Responses");
        public static string DocumentId<T>(this T dto)
        {
            var id = typeof(T).GetProperty("Id");

            if (id == null)
                throw new ArgumentException($"Type {dto.GetType().FullName} does not have an 'Id' property");

            return id.GetValue(dto).ToString();
        }
        
        

        public static void SubscribeWith<TResponse>(this ISubscriptionManager manager, R.ResponsesPaged<TResponse> dto, Q.QueriesPaged<TResponse> query, string session)
        {
            var key = query.GetCacheKey();
            query.SubscriptionType = query.SubscriptionType ?? ChangeType.All;
            foreach (var record in dto.Records)
            {
                manager.Manage(record, key, query.SubscriptionId, query.SubscriptionType ?? ChangeType.All, TimeSpan.FromSeconds(query.SubscriptionTime ?? 3600), session);
            }
            manager.Manage<TResponse>( query, query.SubscriptionId, query.SubscriptionType ?? ChangeType.All, TimeSpan.FromSeconds(query.SubscriptionTime ?? 3600), session);
        }
        public static void SubscribeWith<TResponse>(this ISubscriptionManager manager, R.ResponsesQuery<TResponse> dto, Q.QueriesQuery<TResponse> query, string session)
        {
            var key = query.GetCacheKey();
            manager.Manage(dto.Payload, key, query.SubscriptionId, query.SubscriptionType ?? ChangeType.All, TimeSpan.FromSeconds(query.SubscriptionTime ?? 3600), session);

        }
    }
}