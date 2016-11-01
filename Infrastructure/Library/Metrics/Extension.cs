using Metrics;
using Demo.Library.Demo;
using System;

namespace Demo.Library.Metrics
{
    public static class Extension
    {
        public static MetricsConfig WithDemo(this MetricsConfig config, IDemo pulse, string @event, TimeSpan interval)
        {
            return config.WithReporting(x => x.WithReport(new DemoReport(pulse, @event), interval));
        }
    }
}
