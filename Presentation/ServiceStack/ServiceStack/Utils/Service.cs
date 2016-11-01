using NLog;
using NServiceBus;
using Demo.Library.Utility;
using Demo.Presentation.ServiceStack.Infrastructure.Services;
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using ServiceStack;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Utils
{
    public class Service : DemoService
    {
        private readonly ISubscriptionManager _manager;
        private readonly IMessageSession _bus;

        public Service(ISubscriptionManager manager, IMessageSession bus)
        {
            _manager = manager;
            _bus = bus;
        }

        public object Any(Services.Cpu request)
        {

            var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            var processCpuCounter = new PerformanceCounter("Process", "% Processor Time", Process.GetCurrentProcess().ProcessName);

            processCpuCounter.NextValue();
            cpuCounter.NextValue();

            System.Threading.Thread.Sleep(100);

            return new Responses.Cpu { ProcessUsage = processCpuCounter.NextValue(), TotalUsage = cpuCounter.NextValue() };            
        }
        
        public object Any(Services.Memory request)
        {
            var available = new PerformanceCounter("Memory", "Available MBytes");
            var used = new PerformanceCounter("Process", "Working Set", Process.GetCurrentProcess().ProcessName);

            return new Responses.Memory { FreeBytes = available.NextValue(), ProcessBytes = used.NextValue() };
        }
        public async Task<object> Any(Services.Subscriptions request)
        {
            var subscriptions = await _manager.GetSubscriptions().ConfigureAwait(false);
            return subscriptions.Skip(request.Skip ?? 0).Take(request.Take ?? int.MaxValue)
                .GroupBy(x => x.Document)
                .Select(x => new Responses.Subscriptions
                {
                    Document = x.Key,
                    Sessions = x.Select(y => new Responses.Subscription
                    {
                        CacheKey = y.CacheKey,
                        DocumentId = y.DocumentId,
                        Session = y.Session,
                        Expires = y.Expires
                    })
                });
        }
        public object Any(Services.Clients request)
        {
            return new object();
        }

        public void Delete(ServiceStack.Utils.Services.CacheClear request)
        {
            base.Cache.FlushAll();
        }
        public void Delete(ServiceStack.Utils.Services.DropSubscription request)
        {
            _manager.Drop(request.SubscriptionId);
        }
        public void Delete(ServiceStack.Utils.Services.DropForSession request)
        {
            _manager.DropForSession(Request.GetSessionId());
        }
        public void Any(ServiceStack.Utils.Services.PauseForSession request)
        {
            _manager.PauseForSession(Request.GetSessionId(), request.Pause);
        }
        public void Any(Services.ChangeLogLevel request)
        {
            switch(request.LogLevel.ToUpper())
            {
                case "TRACE":
                    _bus.Publish<IChangeLogLevel>(x =>
                    {
                        x.Level = LogLevel.Trace;
                    });
                    return;
                case "DEBUG":
                    _bus.Publish<IChangeLogLevel>(x =>
                    {
                        x.Level = LogLevel.Debug;
                    });
                    return;
                case "INFO":
                    _bus.Publish<IChangeLogLevel>(x =>
                    {
                        x.Level = LogLevel.Info;
                    });
                    return;
                case "WARN":
                    _bus.Publish<IChangeLogLevel>(x =>
                    {
                        x.Level = LogLevel.Warn;
                    });
                    return;
                case "ERROR":
                    _bus.Publish<IChangeLogLevel>(x =>
                    {
                        x.Level = LogLevel.Error;
                    });
                    return;
                case "FATAL":
                    _bus.Publish<IChangeLogLevel>(x =>
                    {
                        x.Level = LogLevel.Fatal;
                    });
                    return;
            }

            throw new ArgumentException($"Unknown log level [{request.LogLevel}]");
        }
    }
}