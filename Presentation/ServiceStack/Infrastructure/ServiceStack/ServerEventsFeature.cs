using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ServiceStack.Auth;
using ServiceStack.DataAnnotations;
using ServiceStack.Host.Handlers;
using ServiceStack.Logging;
using ServiceStack.Web;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Infrastructure.ServiceStack
{
    public class ServerEventsFeature : IPlugin
    {
        public string StreamPath { get; set; }
        public string HeartbeatPath { get; set; }
        public string SubscribersPath { get; set; }
        public string UnRegisterPath { get; set; }

        public TimeSpan IdleTimeout { get; set; }
        public TimeSpan HeartbeatInterval { get; set; }
        public TimeSpan HouseKeepingInterval { get; set; }

        public Action<IRequest> OnInit { get; set; }
        public Action<IRequest> OnHeartbeatInit { get; set; }
        public Action<IEventSubscription, IRequest> OnCreated { get; set; }
        public Action<IEventSubscription, Dictionary<string, string>> OnConnect { get; set; }
        public Action<IEventSubscription> OnSubscribe { get; set; }
        public Action<IEventSubscription> OnUnsubscribe { get; set; }
        public Action<IResponse, string> OnPublish { get; set; }
        public Action<IResponse, string> WriteEvent { get; set; }
        public Action<IEventSubscription, Exception> OnError { get; set; }
        public bool NotifyChannelOfSubscriptions { get; set; }
        public bool LimitToAuthenticatedUsers { get; set; }
        public bool ValidateUserAddress { get; set; }

        public ServerEventsFeature()
        {
            StreamPath = "/event-stream";
            HeartbeatPath = "/event-heartbeat";
            UnRegisterPath = "/event-unregister";
            SubscribersPath = "/event-subscribers";

            WriteEvent = (res, frame) =>
            {
                res.OutputStream.Write(frame);
                res.Flush();
            };

            IdleTimeout = TimeSpan.FromSeconds(30);
            HeartbeatInterval = TimeSpan.FromSeconds(10);
            HouseKeepingInterval = TimeSpan.FromSeconds(5);

            NotifyChannelOfSubscriptions = true;
            ValidateUserAddress = true;
        }

        public void Register(IAppHost appHost)
        {
            var container = appHost.GetContainer();

            if (!container.Exists<IServerEvents>())
            {
                var broker = new MemoryServerEvents
                {
                    IdleTimeout = IdleTimeout,
                    HouseKeepingInterval = HouseKeepingInterval,
                    OnSubscribe = OnSubscribe,
                    OnUnsubscribe = OnUnsubscribe,
                    NotifyChannelOfSubscriptions = NotifyChannelOfSubscriptions,
                    //OnError = OnError,
                };
                container.Register<IServerEvents>(broker);
            }

            appHost.RawHttpHandlers.Add(httpReq =>
                httpReq.PathInfo.EndsWith(StreamPath)
                    ? (IHttpHandler)new ServerEventsHandler()
                    : httpReq.PathInfo.EndsWith(HeartbeatPath)
                      ? new ServerEventsHeartbeatHandler()
                      : null);

            if (UnRegisterPath != null)
            {
                appHost.RegisterService(typeof(ServerEventsUnRegisterService), UnRegisterPath);
            }

            if (SubscribersPath != null)
            {
                appHost.RegisterService(typeof(ServerEventsSubscribersService), SubscribersPath);
            }
        }

        internal bool CanAccessSubscription(IRequest req, SubscriptionInfo sub)
        {
            if (!ValidateUserAddress)
                return true;

            return sub.UserAddress == null || sub.UserAddress == req.UserHostAddress;
        }
    }

    public class ServerEventsHandler : HttpAsyncTaskHandler
    {
        public override bool RunAsAsync()
        {
            return true;
        }

        public override Task ProcessRequestAsync(IRequest req, IResponse res, string operationName)
        {
            if (HostContext.ApplyCustomHandlerRequestFilters(req, res))
                return TypeConstants.EmptyTask;

            var feature = HostContext.GetPlugin<ServerEventsFeature>();

            var session = req.GetSession();
            if (feature.LimitToAuthenticatedUsers && !session.IsAuthenticated)
            {
                session.ReturnFailedAuthentication(req);
                return TypeConstants.EmptyTask;
            }

            res.ContentType = MimeTypes.ServerSentEvents;
            res.AddHeader(HttpHeaders.CacheControl, "no-cache");
            res.ApplyGlobalResponseHeaders();
            res.UseBufferedStream = false;
            res.KeepAlive = true;

            if (feature.OnInit != null)
                feature.OnInit(req);

            res.Flush();

            var serverEvents = req.TryResolve<IServerEvents>();
            var userAuthId = session != null ? session.UserAuthId : null;
            var anonUserId = serverEvents.GetNextSequence("anonUser");
            var userId = userAuthId ?? ("-" + anonUserId);
            var displayName = session.GetSafeDisplayName()
                ?? "user" + anonUserId;

            var now = DateTime.UtcNow;
            var subscriptionId = SessionExtensions.CreateRandomSessionId();

            //Handle both ?channel=A,B,C or ?channels=A,B,C
            var channels = new List<string>();
            var channel = req.QueryString["channel"];
            if (!string.IsNullOrEmpty(channel))
                channels.AddRange(channel.Split(','));
            channel = req.QueryString["channels"];
            if (!string.IsNullOrEmpty(channel))
                channels.AddRange(channel.Split(','));

            if (channels.Count == 0)
                channels = EventSubscription.UnknownChannel.ToList();

            var subscription = new EventSubscription(res)
            {
                CreatedAt = now,
                LastPulseAt = now,
                Channels = channels.ToArray(),
                SubscriptionId = subscriptionId,
                UserId = userId,
                UserName = session != null ? session.UserName : null,
                DisplayName = displayName,
                SessionId = req.GetSessionId(),
                IsAuthenticated = session != null && session.IsAuthenticated,
                UserAddress = req.UserHostAddress,
                OnPublish = feature.OnPublish,
                //OnError = feature.OnError,
                Meta = {
                    { "userId", userId },
                    { "displayName", displayName },
                    { "channels", string.Join(",", channels) },
                    { AuthMetadataProvider.ProfileUrlKey, session.GetProfileUrl() ?? AuthMetadataProvider.DefaultNoProfileImgUrl },
                }
            };

            if (feature.OnCreated != null)
                feature.OnCreated(subscription, req);

            if (req.Response.IsClosed)
                return TypeConstants.EmptyTask; //Allow short-circuiting in OnCreated callback

            var heartbeatUrl = feature.HeartbeatPath != null
                ? req.ResolveAbsoluteUrl("~/".CombineWith(feature.HeartbeatPath)).AddQueryParam("id", subscriptionId)
                : null;

            var unRegisterUrl = feature.UnRegisterPath != null
                ? req.ResolveAbsoluteUrl("~/".CombineWith(feature.UnRegisterPath)).AddQueryParam("id", subscriptionId)
                : null;

            heartbeatUrl = AddSessionParamsIfAny(heartbeatUrl, req);
            unRegisterUrl = AddSessionParamsIfAny(unRegisterUrl, req);

            subscription.ConnectArgs = new Dictionary<string, string>(subscription.Meta) {
                {"id", subscriptionId },
                {"unRegisterUrl", unRegisterUrl},
                {"heartbeatUrl", heartbeatUrl},
                {"updateSubscriberUrl", req.ResolveAbsoluteUrl("~/event-subscribers/" + subscriptionId) },
                {"heartbeatIntervalMs", ((long)feature.HeartbeatInterval.TotalMilliseconds).ToString(CultureInfo.InvariantCulture) },
                {"idleTimeoutMs", ((long)feature.IdleTimeout.TotalMilliseconds).ToString(CultureInfo.InvariantCulture)}
            };

            if (feature.OnConnect != null)
                feature.OnConnect(subscription, subscription.ConnectArgs);

            serverEvents.Register(subscription, subscription.ConnectArgs);

            var tcs = new TaskCompletionSource<bool>();

            subscription.OnDispose = _ =>
            {
                try
                {
                    res.EndHttpHandlerRequest(skipHeaders: true);
                }
                catch { }
                tcs.SetResult(true);
            };

            return tcs.Task;
        }

        static string AddSessionParamsIfAny(string url, IRequest req)
        {
            if (url != null && HostContext.Config.AllowSessionIdsInHttpParams)
            {
                var sessionKeys = new[] { "ss-id", "ss-pid", "ss-opt" };
                foreach (var key in sessionKeys)
                {
                    var value = req.QueryString[key];
                    if (value != null)
                        url = url.AddQueryParam(key, value);
                }
            }

            return url;
        }
    }

    public class ServerEventsHeartbeatHandler : HttpAsyncTaskHandler
    {
        public override bool RunAsAsync() { return true; }

        public override Task ProcessRequestAsync(IRequest req, IResponse res, string operationName)
        {
            if (HostContext.ApplyCustomHandlerRequestFilters(req, res))
                return TypeConstants.EmptyTask;

            res.ApplyGlobalResponseHeaders();

            var serverEvents = req.TryResolve<IServerEvents>();

            serverEvents.RemoveExpiredSubscriptions();

            var feature = HostContext.GetPlugin<ServerEventsFeature>();
            if (feature.OnHeartbeatInit != null)
                feature.OnHeartbeatInit(req);

            if (req.Response.IsClosed)
                return TypeConstants.EmptyTask;

            var subscriptionId = req.QueryString["id"];
            var subscription = serverEvents.GetSubscriptionInfo(subscriptionId);
            if (subscription == null)
            {
                res.StatusCode = 404;
                res.StatusDescription = ErrorMessages.SubscriptionNotExistsFmt.Fmt(subscriptionId);
                res.EndHttpHandlerRequest(skipHeaders: true);
                return TypeConstants.EmptyTask;
            }

            if (!feature.CanAccessSubscription(req, subscription))
            {
                res.StatusCode = 403;
                res.StatusDescription = "Invalid User Address";
                res.EndHttpHandlerRequest(skipHeaders: true);
                return TypeConstants.EmptyTask;
            }

            if (!serverEvents.Pulse(subscriptionId))
            {
                res.StatusCode = 404;
                res.StatusDescription = "Subscription {0} does not exist".Fmt(subscriptionId);
            }
            res.EndHttpHandlerRequest(skipHeaders: true);
            return TypeConstants.EmptyTask;
        }
    }
    
}