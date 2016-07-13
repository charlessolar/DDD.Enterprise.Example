using Aggregates.Messages;
using NServiceBus;
using Demo.Library.Exceptions;
using Demo.Library.Reply;
using Demo.Presentation.ServiceStack.Infrastructure.Authentication;
using Demo.Presentation.ServiceStack.Infrastructure.Exceptions;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using ServiceStack;
using ServiceStack.Caching;
using ServiceStack.Configuration;
using ServiceStack.Logging;
using ServiceStack.Text;
using ServiceStack.Web;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Q = Demo.Presentation.ServiceStack.Infrastructure.Queries;
using R = Demo.Presentation.ServiceStack.Infrastructure.Responses;

namespace Demo.Presentation.ServiceStack.Infrastructure.Extensions
{
    public static class RequestExtensions
    {
        private static ILog Logger = LogManager.GetLogger("Requests");
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

        public static R.Responses_Query<TResponse> ToOptimizedCachedResult<TResponse>(this IRequest request, Q.Queries_Query<TResponse> query, ICacheClient cache, Func<R.Responses_Query<TResponse>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.GetOrCreate(key, () =>
            {
                return factory();
            });
            
            return cached;
        }
        public static async Task<R.Responses_Query<TResponse>> ToOptimizedCachedResult<TResponse>(this IRequest request, Q.Queries_Query<TResponse> query, ICacheClient cache, Func<Task<R.Responses_Query<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.Get<R.Responses_Query<TResponse>>(key);
            if (cached == null)
            {
                cached = await factory();
                cache.Add(key, cached);
            }

            return cached;
        }

        public static R.Responses_Query<TResponse> ToOptimizedCachedAndSubscribedResult<TResponse>(this IRequest request, Q.Queries_Query<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<R.Responses_Query<TResponse>> factory)
        {
            var result = request.ToOptimizedCachedResult(query, cache, () =>
            {
                var response = factory();
                return response;
            });
            manager.SubscribeWith(result, query, request.GetSessionId());
            return result;
        }
        public static async Task<R.Responses_Query<TResponse>> ToOptimizedCachedAndSubscribedResult<TResponse>(this IRequest request, Q.Queries_Query<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<Task<R.Responses_Query<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.Get<R.Responses_Query<TResponse>>(key);
            if (cached == null)
            {
                cached = await factory();
                cache.Add(key, cached);
            }
            manager.SubscribeWith(cached, query, request.GetSessionId());

            return cached;
        }

        public static R.Responses_Paged<TResponse> ToOptimizedCachedResult<TResponse>(this IRequest request, Q.Queries_Paged<TResponse> query, ICacheClient cache, Func<R.Responses_Paged<TResponse>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.GetOrCreate(key, () =>
            {
                var result = factory();
                return result;
            });
            
            return cached;
        }
        public static async Task<R.Responses_Paged<TResponse>> ToOptimizedCachedResult<TResponse>(this IRequest request, Q.Queries_Paged<TResponse> query, ICacheClient cache, Func<Task<R.Responses_Paged<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.Get<R.Responses_Paged<TResponse>>(key);
            if(cached == null)
            {
                cached = await factory();
                cache.Add(key, cached);
            }

            return cached;
        }

        public static R.Responses_Paged<TResponse> ToOptimizedCachedAndSubscribedPagedResult<TResponse>(this IRequest request, Q.Queries_Paged<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<R.Responses_Paged<TResponse>> factory)
        {
            var result = request.ToOptimizedCachedResult(query, cache, () =>
            {
                var response = factory();
                return response;
            });
            manager.SubscribeWith(result, query, request.GetSessionId());
            return result;
        }
        public static async Task<R.Responses_Paged<TResponse>> ToOptimizedCachedAndSubscribedPagedResult<TResponse>(this IRequest request, Q.Queries_Paged<TResponse> query, ICacheClient cache, ISubscriptionManager manager, Func<Task<R.Responses_Paged<TResponse>>> factory)
        {
            var key = query.GetCacheKey();
            var cached = cache.Get<R.Responses_Paged<TResponse>>(key);
            if (cached == null)
            {
                cached = await factory();
                cache.Add(key, cached);
            }
            manager.SubscribeWith(cached, query, request.GetSessionId());

            return cached;
        }
        public static Task<IReply> IsQuery<TResponse>(this ICallback callback) where TResponse : class
        {
            return callback.Register(x =>
            {
                var reply = x.Messages.FirstOrDefault();
                if (reply == null || reply is Reject)
                {
                    var reject = reply as Reject;
                    Logger.WarnFormat("Query was rejected - Message: {0}\nException: {1}", reject.Message, reject.Exception);
                    if (reject != null && reject.Exception != null)
                        throw new QueryRejectedException(reject.Message, reject.Exception);
                    else if (reject != null)
                        throw new QueryRejectedException(reject.Message);
                    throw new QueryRejectedException();
                }
                if(reply is Error)
                {
                    var error = reply as Error;
                    Logger.WarnFormat("Query raised an error - Message: {0}", error.Message);
                    throw new QueryRejectedException(error.Message);
                }

                var package = reply as IReply;
                if (package == null)
                    throw new QueryRejectedException($"Unexpected response type: {reply.GetType().FullName}");
                return package;
            });
        }
        public static Task<IPagedReply> IsPaged<TResponse>(this ICallback callback) where TResponse : class
        {
            return callback.Register(x =>
            {
                var reply = x.Messages.FirstOrDefault();
                if (reply == null || reply is Reject)
                {
                    var reject = reply as Reject;
                    Logger.WarnFormat("Query was rejected - Message: {0}\nException: {1}", reject.Message, reject.Exception);
                    if (reject != null && reject.Exception != null)
                        throw new QueryRejectedException(reject.Message, reject.Exception);
                    else if (reject != null)
                        throw new QueryRejectedException(reject.Message);
                    throw new QueryRejectedException();
                }
                if (reply is Error)
                {
                    var error = reply as Error;
                    Logger.WarnFormat("Query raised an error - Message: {0}", error.Message);
                    throw new QueryRejectedException(error.Message);
                }

                var package = reply as IPagedReply;
                if (package == null)
                    throw new QueryRejectedException($"Unexpected response type: {reply.GetType().FullName}");
                
                return package;
            });
        }
    }
}