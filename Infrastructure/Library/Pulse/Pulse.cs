using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Demo.Library.Extensions;
using Newtonsoft.Json;
using ServiceStack;
using Metrics;
using NLog;

namespace Demo.Library.Demo
{
    public class Demo : IDemo
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static readonly Meter ReportsMeter = Metric.Meter("Demo Reports", Unit.Items);

        private string _url;
        private Guid _stashId;
        private string _stashSecret;

        public async Task Init(string url, Guid stashId, string secret, string name, string description)
        {
            _url = url;
            _stashId = stashId;
            _stashSecret = secret;
            Logger.Info("Initiating pulse reporter to url {0} stash id {1} name {2} description {3}", url, stashId, name, description);

            var client = new RestClient(url);

            var init = new RestRequest("stash", Method.POST);
            init.AddParameter("StashId", stashId);
            init.AddParameter("Secret", secret);
            init.AddParameter("Name", name);
            init.AddParameter("Description", description);

            await client.ExecuteAsync(init).ConfigureAwait(false);

        }
        public Task Report(string Event, object data)
        {
            var dict = data.ToObjectDictionary();
            return Report(Event, dict);
        }
        public Task Report(string Event, IDictionary<string, object> data)
        {
            return Send(Event, data);
        }

        private async Task Send(string Event, IEnumerable<KeyValuePair<string, object>> properties)
        {
            if (string.IsNullOrEmpty(_url)) return;

            //Logger.Debug("Sending pulse report to stash id {0} event {1}", _stashId, Event);

            var client = new RestClient(_url);
            var request = new RestRequest("stash/{StashId}/event", Method.POST);

            request.AddUrlSegment("StashId", _stashId.ToString("N"));

            request.AddParameter("Secret", _stashSecret);
            request.AddParameter("Name", Event);
            request.AddParameter("Source", Environment.MachineName);
            request.AddParameter("Timestamp", DateTime.UtcNow.ToUnix());

            var data = properties.ToDictionary(x => x.Key, x => x.Value);

            request.AddParameter("Data", JsonConvert.SerializeObject(data));

            ReportsMeter.Mark();
            await client.ExecuteAsync(request).ConfigureAwait(false);
        }
    }
}
