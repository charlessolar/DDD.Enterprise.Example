using Aggregates.Messages;
using NServiceBus;
using Demo.Library.Exceptions;
using Demo.Library.Reply;
using Demo.Presentation.ServiceStack.Infrastructure.Authentication;
using Demo.Presentation.ServiceStack.Infrastructure.Commands;
using Demo.Library.SSE;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Web;
using System;
using System.Linq;
using System.Threading.Tasks;
using Q = Demo.Presentation.ServiceStack.Infrastructure.Queries;
using R = Demo.Presentation.ServiceStack.Infrastructure.Responses;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class RequestExtensions
    {
        private static readonly ILog Logger = LogManager.GetLogger("Requests");
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

                if (token[0].ToUpper() != "PULSEAUTH")
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

        public static R.ResponsesQuery<TResponse> ToOptimizedCachedResult<TResponse>(this IRequest request, Q.QueriesQuery<TResponse> query, ICacheClient cache, Func<R.ResponsesQuery<TResponse>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.GetOrCreate(key, factory);

            return cached;
        }
        public static async Task<R.ResponsesQuery<TResponse>> ToOptimizedCachedResult<TResponse>(this IRequest request, Q.QueriesQuery<TResponse> query, ICacheClient cache, Func<Task<R.ResponsesQuery<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.Get<R.ResponsesQuery<TResponse>>(key);
            if (cached == null)
            {
                cached = await factory().ConfigureAwait(false);
                cache.Add(key, cached);
            }

            return cached;
        }

        public static R.ResponsesQuery<TResponse> ToOptimizedCachedAndSubscribedResult<TResponse>(this IRequest request, Q.QueriesQuery<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<R.ResponsesQuery<TResponse>> factory)
        {
            var result = request.ToOptimizedCachedResult(query, cache, () =>
            {
                var response = factory();
                return response;
            });
            if (!query.SubscriptionId.IsNullOrEmpty())
                manager.SubscribeWith(result, query, request.GetSessionId());
            return result;
        }
        public static async Task<R.ResponsesQuery<TResponse>> ToOptimizedCachedAndSubscribedResult<TResponse>(this IRequest request, Q.QueriesQuery<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<Task<R.ResponsesQuery<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.Get<R.ResponsesQuery<TResponse>>(key);
            if (cached == null)
            {
                cached = await factory().ConfigureAwait(false);
                cache.Add(key, cached);
            }
            if (!query.SubscriptionId.IsNullOrEmpty())
                manager.SubscribeWith(cached, query, request.GetSessionId());

            return cached;
        }

        public static R.ResponsesPaged<TResponse> ToOptimizedCachedResult<TResponse>(this IRequest request, Q.QueriesPaged<TResponse> query, ICacheClient cache, Func<R.ResponsesPaged<TResponse>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.GetOrCreate(key, () =>
            {
                var result = factory();
                return result;
            });

            return cached;
        }
        public static async Task<R.ResponsesPaged<TResponse>> ToOptimizedCachedResult<TResponse>(this IRequest request, Q.QueriesPaged<TResponse> query, ICacheClient cache, Func<Task<R.ResponsesPaged<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.Get<R.ResponsesPaged<TResponse>>(key);
            if (cached == null)
            {
                cached = await factory().ConfigureAwait(false);
                cache.Add(key, cached);
            }

            return cached;
        }

        public static R.ResponsesPaged<TResponse> ToOptimizedCachedAndSubscribedPagedResult<TResponse>(this IRequest request, Q.QueriesPaged<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<R.ResponsesPaged<TResponse>> factory)
        {
            var result = request.ToOptimizedCachedResult(query, cache, () =>
            {
                var response = factory();
                return response;
            });
            if (!query.SubscriptionId.IsNullOrEmpty())
                manager.SubscribeWith(result, query, request.GetSessionId());
            return result;
        }
        public static async Task<R.ResponsesPaged<TResponse>> ToOptimizedCachedAndSubscribedPagedResult<TResponse>(this IRequest request, Q.QueriesPaged<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<Task<R.ResponsesPaged<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.Get<R.ResponsesPaged<TResponse>>(key);
            if (cached == null)
            {
                cached = await factory().ConfigureAwait(false);
                cache.Add(key, cached);
            }
            if (!query.SubscriptionId.IsNullOrEmpty())
                manager.SubscribeWith(cached, query, request.GetSessionId());

            return cached;
        }
        public static R.ResponsesQuery<TResponse> RequestQuery<TResponse>(this IMessage message, Q.QueriesQuery<TResponse> query = null) where TResponse : class
        {

            if (message == null || message is Reject)
            {
                var reject = (Reject) message;
                Logger.WarnFormat("Query was rejected - Message: {0}\n", reject.Message);
                if (reject != null)
                    throw new QueryRejectedException(reject.Message);
                throw new QueryRejectedException();
            }
            if (message is Error)
            {
                var error = (Error) message;
                Logger.WarnFormat("Query raised an error - Message: {0}", error.Message);
                throw new QueryRejectedException(error.Message);
            }

            var package = (IReply) message;
            if (package == null)
                throw new QueryRejectedException($"Unexpected response type: {message.GetType().FullName}");

            var payload = package.Payload.ConvertTo<TResponse>();

            return new R.ResponsesQuery<TResponse>
            {
                Payload = payload,
                Etag = package.ETag,
                SubscriptionId = query?.SubscriptionId,
                SubscriptionTime = query?.SubscriptionTime,
            };
        }
        public static R.ResponsesPaged<TResponse> RequestPaged<TResponse>(this IMessage message, Q.QueriesPaged<TResponse> query = null) where TResponse : class
        {

            if (message == null || message is Reject)
            {
                var reject = (Reject) message;
                Logger.WarnFormat("Query was rejected - Message: {0}\n", reject.Message);
                if (reject != null)
                    throw new QueryRejectedException(reject.Message);
                throw new QueryRejectedException();
            }
            if (message is Error)
            {
                var error = (Error) message;
                Logger.WarnFormat("Query raised an error - Message: {0}", error.Message);
                throw new QueryRejectedException(error.Message);
            }

            var package = (IPagedReply) message;
            if (package == null)
                throw new QueryRejectedException($"Unexpected response type: {message.GetType().FullName}");

            var records = package.Records.Select(x => x.ConvertTo<TResponse>());

            return new R.ResponsesPaged<TResponse>
            {
                Records = records,
                Total = package.Total,
                ElapsedMs = package.ElapsedMs,
                SubscriptionId = query?.SubscriptionId,
                SubscriptionTime = query?.SubscriptionTime,
            };
        }

        public static Task SubscribeCommand<TResponse>(this IRequest request, string documentId, ServiceCommand command, ISubscriptionManager manager)
        {
            if (!command.SubscriptionId.IsNullOrEmpty())
                return manager.Manage<TResponse>(documentId, command.SubscriptionId, command.SubscriptionType ?? ChangeType.All, TimeSpan.FromSeconds(command.SubscriptionTime ?? 300), request.GetSessionId());
            return Task.FromResult(0);
        }
    }
}