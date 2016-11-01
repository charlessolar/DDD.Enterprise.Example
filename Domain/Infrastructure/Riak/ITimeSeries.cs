using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Domain.Infrastructure.Riak
{
    public class Datapoint
    {
        public string Name { get; set; }
        public object Value { get; set; }
        public long Timestamp { get; set; }
        public IDictionary<string, string> Tags { get; set; }
    }
    public interface ITimeSeries
    {
        Task<bool> SaveMetric(string streamId, string name, object value, long timestamp, IDictionary<string, string> tags);
        Task<bool> SaveMetrics(string streamId, IEnumerable<Datapoint> metrics);

        Task<IEnumerable<Datapoint>> Retrieve(string streamId, long? @from = null, long? to = null);
    }
}
