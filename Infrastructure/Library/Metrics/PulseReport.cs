using Metrics;
using Metrics.MetricData;
using Metrics.Reporters;
using Metrics.Utils;
using Demo.Library.Demo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Demo.Library.Extensions;

namespace Demo.Library.Metrics
{
    public class DemoReport : BaseReport
    {
        private static readonly Regex Invalid = new Regex(@"[^a-zA-Z0-9\-%&]+", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex InvalidAllowDots = new Regex(@"[^a-zA-Z0-9\-%&.]+", RegexOptions.CultureInvariant | RegexOptions.Compiled);
        private static readonly Regex Slash = new Regex(@"\s*/\s*", RegexOptions.CultureInvariant | RegexOptions.Compiled);

        private readonly IDemo _sender;
        private readonly string _event;
        private readonly object _queueLock = new object();
        private readonly IDictionary<string, object> _queued;
        private string _contextName;

        public DemoReport(IDemo sender, string @event)
        {
            this._sender = sender;
            this._event = @event;
            this._queued = new Dictionary<string, object>();
        }

        protected override void StartReport(string contextName)
        {
            base.StartReport(contextName);

            _contextName = contextName;
        }

        protected override void EndReport(string contextName)
        {
            base.EndReport(contextName);

            lock (_queueLock)
            {
                _queued["Timestamp"] = this.CurrentContextTimestamp.ToUnix();

                this._sender.Report(_event, _queued).Wait();
                _queued.Clear();
            }
        }

        protected override void ReportGauge(string name, double value, Unit unit, MetricTags tags)
        {
            if (!double.IsNaN(value) && !double.IsInfinity(value))
            {
                Send(Name(name, unit), value);
            }
        }

        protected override void ReportCounter(string name, CounterValue value, Unit unit, MetricTags tags)
        {
            if (value.Items.Length == 0)
            {
                Send(Name(name, unit), value.Count);
            }
            else
            {
                Send(SubfolderName(name, unit, "Total"), value.Count);
            }

            foreach (var item in value.Items)
            {
                Send(SubfolderName(name, unit, item.Item), item.Count);
                Send(SubfolderName(name, unit, item.Item, "Percent"), item.Percent);
            }
        }

        protected override void ReportMeter(string name, MeterValue value, Unit unit, TimeUnit rateUnit, MetricTags tags)
        {
            Send(SubfolderName(name, unit, "Total"), value.Count);
            Send(SubfolderName(name, AsRate(unit, rateUnit), "Rate-Mean"), value.MeanRate);
            Send(SubfolderName(name, AsRate(unit, rateUnit), "Rate-1-min"), value.OneMinuteRate);
            Send(SubfolderName(name, AsRate(unit, rateUnit), "Rate-5-min"), value.FiveMinuteRate);
            Send(SubfolderName(name, AsRate(unit, rateUnit), "Rate-15-min"), value.FifteenMinuteRate);

            foreach (var item in value.Items)
            {
                Send(SubfolderName(name, unit, item.Item, "Count"), item.Percent);
                Send(SubfolderName(name, unit, item.Item, "Percent"), item.Value.Count);
                Send(SubfolderName(name, AsRate(unit, rateUnit), item.Item, "Rate-Mean"), item.Value.MeanRate);
                Send(SubfolderName(name, AsRate(unit, rateUnit), item.Item, "Rate-1-min"), item.Value.OneMinuteRate);
                Send(SubfolderName(name, AsRate(unit, rateUnit), item.Item, "Rate-5-min"), item.Value.FiveMinuteRate);
                Send(SubfolderName(name, AsRate(unit, rateUnit), item.Item, "Rate-15-min"), item.Value.FifteenMinuteRate);
            }
        }

        protected override void ReportHistogram(string name, HistogramValue value, Unit unit, MetricTags tags)
        {
            Send(SubfolderName(name, unit, "Count"), value.Count);
            Send(SubfolderName(name, unit, "Last"), value.LastValue);
            Send(SubfolderName(name, unit, "Min"), value.Min);
            Send(SubfolderName(name, unit, "Mean"), value.Mean);
            Send(SubfolderName(name, unit, "Max"), value.Max);
            Send(SubfolderName(name, unit, "StdDev"), value.StdDev);
            Send(SubfolderName(name, unit, "Median"), value.Median);
            Send(SubfolderName(name, unit, "p75"), value.Percentile75);
            Send(SubfolderName(name, unit, "p95"), value.Percentile95);
            Send(SubfolderName(name, unit, "p98"), value.Percentile98);
            Send(SubfolderName(name, unit, "p99"), value.Percentile99);
            Send(SubfolderName(name, unit, "p99,9"), value.Percentile999);
        }

        protected override void ReportTimer(string name, TimerValue value, Unit unit, TimeUnit rateUnit, TimeUnit durationUnit, MetricTags tags)
        {
            Send(SubfolderName(name, unit, "Count"), value.Rate.Count);
            Send(SubfolderName(name, unit, "Active_Sessions"), value.ActiveSessions);

            Send(SubfolderName(name, AsRate(unit, rateUnit), "Rate-Mean"), value.Rate.MeanRate);
            Send(SubfolderName(name, AsRate(unit, rateUnit), "Rate-1-min"), value.Rate.OneMinuteRate);
            Send(SubfolderName(name, AsRate(unit, rateUnit), "Rate-5-min"), value.Rate.FiveMinuteRate);
            Send(SubfolderName(name, AsRate(unit, rateUnit), "Rate-15-min"), value.Rate.FifteenMinuteRate);

            Send(SubfolderName(name, durationUnit.Unit(), "Duration-Last"), value.Histogram.LastValue);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-Min"), value.Histogram.Min);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-Mean"), value.Histogram.Mean);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-Max"), value.Histogram.Max);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-StdDev"), value.Histogram.StdDev);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-Median"), value.Histogram.Median);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-p75"), value.Histogram.Percentile75);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-p95"), value.Histogram.Percentile95);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-p98"), value.Histogram.Percentile98);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-p99"), value.Histogram.Percentile99);
            Send(SubfolderName(name, durationUnit.Unit(), "Duration-p999"), value.Histogram.Percentile999);
        }

        protected override void ReportHealth(HealthStatus status)
        { }

        protected virtual void Send(string name, long value)
        {
            lock (_queueLock) this._queued[name] = value;
        }

        protected virtual void Send(string name, double value)
        {
            lock (_queueLock) this._queued[name] = value;
        }

        protected virtual void Send(string name, string value)
        {
            lock(_queueLock) this._queued[name] = value;
        }

        protected override string FormatContextName(IEnumerable<string> contextStack, string contextName)
        {
            var parts = contextStack.Concat(new[] { contextName })
                .Select(_ => GraphiteName(_, true));

            return string.Join(":", parts);
        }

        protected override string FormatMetricName<T>(string context, MetricValueSource<T> metric)
        {
            return string.Concat(context, ":", GraphiteName(metric.Name));
        }


        protected virtual string AsRate(Unit unit, TimeUnit rateUnit)
        {
            return string.Concat(unit.Name, "-per-", rateUnit.Unit());
        }

        protected virtual string SubfolderName(string cleanName, Unit unit, string itemName, string itemSuffix)
        {
            return Name(string.Concat(cleanName, ":", GraphiteName(itemName), "-", GraphiteName(itemSuffix)), unit);
        }

        protected virtual string SubfolderName(string cleanName, Unit unit, string itemSuffix)
        {
            return Name(string.Concat(cleanName, ":", GraphiteName(itemSuffix)), unit);
        }

        protected virtual string Name(string cleanName, Unit unit, string itemSuffix)
        {
            return Name(string.Concat(cleanName, "-", GraphiteName(itemSuffix)), unit);
        }


        protected virtual string Name(string cleanName, Unit unit)
        {
            return Name(cleanName, unit.Name);
        }

        protected virtual string Name(string cleanName, string unit)
        {
            cleanName = cleanName.Replace(_contextName, "");
            return string.Concat(cleanName, FormatUnit(unit, cleanName));
        }

        protected virtual string FormatUnit(string unit, string name)
        {
            if (string.IsNullOrEmpty(unit))
            {
                return string.Empty;
            }
            var clean = GraphiteName(unit);

            if (string.IsNullOrEmpty(clean))
            {
                return string.Empty;
            }

            if (name.EndsWith(clean, StringComparison.InvariantCultureIgnoreCase))
            {
                return string.Empty;
            }

            return string.Concat("-", clean);
        }

        protected virtual string GraphiteName(string name, bool allowDots = false)
        {
            var noSlash = Slash.Replace(name, "-per-");
            return allowDots ?
                InvalidAllowDots.Replace(noSlash, "_").Trim('_') :
                Invalid.Replace(noSlash, "_").Trim('_');
        }
        
        
    }
}
