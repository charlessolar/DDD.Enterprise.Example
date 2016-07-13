
using System;
using System.Diagnostics;
using System.Web;
using Metrics;
using Metrics.Core;
using ServiceStack;

namespace Demo.Presentation.ServiceStack.Profiling
{
    

    public class MetricsFeature : IPlugin
    {
        private Meter _requestsMeter = Metric.Meter( "Requests", Unit.Requests);
        private Timer _requestsTimer = Metric.Timer("Requests Duration", Unit.Requests);
        private Counter _requestsConcurrent = Metric.Counter("Concurrent Requests", Unit.Requests);

        private Meter _errorsMeter = Metric.Meter("Request Errors", Unit.Errors);

        private class RequestTimer : IDisposable
        {
            private readonly Counter _counter;
            private readonly TimerContext _context;

            private RequestTimer(Timer timer, Counter concurrent)
            {
                _counter = concurrent;
                _context = timer.NewContext();
                _counter.Increment();
            }

            public static RequestTimer Start(Timer timer, Counter concurrent)
            {
                return new RequestTimer(timer, concurrent);
            }
            public void Dispose()
            {
                _counter.Decrement();
                _context.Dispose();
            }
        }

        public void Register(IAppHost appHost)
        {
            appHost.PreRequestFilters.Insert(0, (request, response) =>
            {
                _requestsMeter.Mark();
                ((HttpRequestBase)request.OriginalRequest).RequestContext.HttpContext.Items.Add("_metrics_timer", RequestTimer.Start(_requestsTimer, _requestsConcurrent));
            });

            appHost.ServiceExceptionHandlers.Add((req, request, exception) =>
            {
                _errorsMeter.Mark();
                return null;
            });
            appHost.UncaughtExceptionHandlers.Add((req, res, name, exception) => _errorsMeter.Mark());
        }
    }
}