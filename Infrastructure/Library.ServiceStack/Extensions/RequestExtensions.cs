using Demo.Library.Authentication;
using Demo.Library.Queries.Processor;
using Demo.Library.SSE;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Text;
using ServiceStack.Web;
using System;
using System.IO;
using Q = Demo.Library.Queries;
using R = Demo.Library.Responses;

namespace Demo.Library.Extensions
{
    public static class RequestExtensions
    {
        public static Auth0Profile RetreiveUserProfile(this IRequest request)
        {
            var appSettings = new AppSettings();
            var appSecret = appSettings.GetString("oauth.auth0.AppSecret").Replace('-', '+').Replace('_', '/');

            var header = request.Headers["Authorization"];

            if (header.IsNullOrEmpty())
                return null;
            try
            {
                var token = header.Split(' ');

                if (token[0].ToUpper() != "FORTEAUTH")
                    return null;

                var profile = JWT.JsonWebToken.Decode(token[1], Convert.FromBase64String(appSecret), verify: true);
                if (profile.IsNullOrEmpty())
                    return null;

                var auth0Profile = profile.FromJson<Auth0Profile>();

                return auth0Profile;
            }
            catch
            {
                return null;
            }
        }

        public static Object ToOptimizedCachedAndSubscribedResult<TResponse>(this IRequest request, Q.Query<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<R.Query<TResponse>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.GetOrCreate(key, () =>
            {
                return factory();
            });

            // Send response to client, so our subscription stuff doesn't delay the client
            request.Response.ContentType = request.ResponseContentType;
            request.Response.WriteToResponse(request, cached);
            request.Response.EndRequest();

            var documentId = "";
            var idField = typeof(TResponse).GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (idField == null)
                documentId = idField.GetValue(cached.Payload).ToString();

            manager.Manage<TResponse>(key, documentId, query.SubscriptionId, request.GetPermanentSessionId());

            return cached;
        }

        public static Object ToOptimizedCachedAndSubscribedResult<TResponse>(this IRequest request, Q.Query<TResponse> query, ICacheClient cache, ISubscriptionManager manager, IQueryProcessor processor)
        {
            return request.ToOptimizedCachedAndSubscribedResult(query, cache, manager, () =>
            {
                return processor.Process(query);
            });
        }

        public static Object ToOptimizedCachedAndSubscribedResult<TResponse>(this IRequest request, Q.PagedQuery<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<R.Query<R.Paged<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.GetOrCreate(key, () =>
            {
                return factory();
            });

            // Send response to client, so our subscription stuff doesn't delay the client
            request.Response.ContentType = request.ResponseContentType;
            request.Response.WriteToResponse(request, cached);
            request.Response.EndRequest();

            var documentId = "";
            var idField = typeof(TResponse).GetProperty("Id", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (var each in cached.Payload.Records)
            {
                if (idField != null)
                    documentId = idField.GetValue(each).ToString();

                manager.Manage<TResponse>(key, documentId, query.SubscriptionId, request.GetPermanentSessionId());
            }

            if (!query.SubscriptionId.IsNullOrEmpty())
                manager.Manage<TResponse>(key, "", query.SubscriptionId, request.GetPermanentSessionId());

            return cached;
        }
        public static Object ToOptimizedCachedAndSubscribedResult<TResponse>(this IRequest request, Q.PagedQuery<TResponse> query, ICacheClient cache, ISubscriptionManager manager, IQueryProcessor processor)
        {
            return request.ToOptimizedCachedAndSubscribedResult(query, cache, manager, () =>
            {
                return processor.Process(query);
            });
        }
    }
}