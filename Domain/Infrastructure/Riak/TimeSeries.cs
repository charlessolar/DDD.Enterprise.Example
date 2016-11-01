using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RiakClient;
using NLog;
using Demo.Library.Extensions;
using RiakClient.Models;
using NServiceBus;
using Demo.Library.Future;
using Demo.Domain.Infrastructure.Demo;
using System.Collections.Concurrent;

namespace Demo.Domain.Infrastructure.Riak
{
    public class TimeSeries :
        ITimeSeries,
        IHandleMessages<ReduceMetrics>
    {
        private class Quantum
        {
            public string Id
            {
                get
                {
                    var tags = string.Join(";", Tags.Select(x => x.Key + "=" + x.Value));
                    return $"{Name}.{tags}.{Timestamp}.{Duration}";
                }
            }
            public string Name { get; set; }
            public long Timestamp { get; set; }
            public string Duration { get; set; }

            public decimal Value { get; set; }
            public int Count { get; set; }

            public IDictionary<string, string> Tags { get; set; }
        }

        private static NLog.ILogger _logger = LogManager.GetCurrentClassLogger();
        private static bool _first = true;
        private static ConcurrentBag<string> _updatedNames = new ConcurrentBag<string>();

        private readonly IRiakClient _client;

        public TimeSeries(IRiakClient client, IFuture future)
        {
            _client = client;

            if (!_first) return;

            _first = false;

            //future.FireRepeatedly(TimeSpan.FromMinutes(5), dispatcher, (state) =>
            //{
            //    (state as IDispatcher).Dispatch<ReduceMetrics>(x => { });
            //}, Description: "Runs metric reducing to reduce space used by older metrics");
        }

        public async Task Handle(ReduceMetrics e, IMessageHandlerContext ctx)
        {
            var updated = _updatedNames.ToList();
            _updatedNames = new ConcurrentBag<string>();

            foreach (var update in updated)
            {
                var timeDbId = new RiakObjectId("default", "Timeseries", update);

                var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
                options.SetRw(Quorum.WellKnown.Quorum);
                _logger.Debug("Getting timeseries db of id {0}", timeDbId.Key);
                var result = await _client.Async.Get(timeDbId, options).ConfigureAwait(false);

                if (!result.IsSuccess)
                    continue;

                var timedb = result.Value.GetObject<IEnumerable<Quantum>>();
                var directs = timedb.Where(x => x.Duration == "x");
                var timeDiff = TimeSpan.FromSeconds(directs.Select(x => x.Timestamp).Aggregate((cur, next) => Math.Abs(cur - next)) / directs.Count());


                var levelOneBucket = TimeSpan.FromMinutes(timeDiff.Seconds);
                {

                    var tooOld = DateTime.UtcNow.ToUnix() - (levelOneBucket.Seconds * 500);
                    var levelOne = timedb.Where(x => x.Duration=="x" && x.Timestamp < tooOld);

                    var toAddOrUpdate = new List<Quantum>();
                    _logger.Debug("Reducing {0} metrics into buckets {1}m", levelOne.Count(), levelOneBucket.TotalMinutes);
                    foreach (var group in levelOne.GroupBy(x => new { Bucket= x.Timestamp % levelOneBucket.Seconds, Tags = x.Tags }))
                    {
                        var quantum = new Quantum
                        {
                            Timestamp = group.First().Timestamp,
                            Duration = $"{levelOneBucket.TotalMinutes}m",
                            Name = update,
                            Value = group.Average(x => x.Value),
                            Count = group.Count(),
                            Tags = group.Key.Tags
                        };
                        toAddOrUpdate.Add(quantum);
                    }
                    
                    var success = await UpdateTimeDb(update, (db) =>
                    {
                        levelOne.ForEach(x => db = db.TryRemove(x, y => y.Id));

                        foreach (var toadd in toAddOrUpdate)
                        {

                            var existing = db.SingleOrDefault(x => x.Id == toadd.Id);
                            if (existing == null)
                                db = db.Add(toadd);
                            else
                            {
                                existing.Value = ((existing.Value * existing.Count) + (toadd.Value * toadd.Count)) / (existing.Count + toadd.Count);
                                existing.Count += toadd.Count;
                            }
                        }
                        return db;
                    });
                    if (!success)
                        _logger.Warn("Failed to reduce {0} metrics into buckets {1}m", levelOne.Count(), levelOneBucket.TotalMinutes);
                }
                var levelTwoBucket = TimeSpan.FromHours(timeDiff.Seconds);
                {

                    var tooOld = DateTime.UtcNow.ToUnix() - (levelTwoBucket.Seconds * 500);
                    var levelTwo = timedb.Where(x => x.Duration != $"{levelTwoBucket.TotalHours}h" && x.Timestamp < tooOld);

                    var toAddOrUpdate = new List<Quantum>();
                    _logger.Debug("Reducing {0} metrics into buckets {1}h", levelTwo.Count(), levelTwoBucket.TotalHours);
                    foreach (var group in levelTwo.GroupBy(x => new { Bucket = x.Timestamp % levelTwoBucket.Seconds, Tags = x.Tags }))
                    {
                        var quantum = new Quantum
                        {
                            Timestamp = group.First().Timestamp,
                            Duration = $"{levelTwoBucket.TotalHours}h",
                            Name = update,
                            Value = group.Average(x => x.Value),
                            Count = group.Count(),
                            Tags = group.Key.Tags
                        };
                        toAddOrUpdate.Add(quantum);
                    }
                    var success = await UpdateTimeDb(update, (db) =>
                    {
                        levelTwo.ForEach(x => db = db.TryRemove(x, y => y.Id));

                        foreach (var toadd in toAddOrUpdate)
                        {
                            var existing = db.SingleOrDefault(x => x.Id == toadd.Id);
                            if (existing == null)
                                db = db.Add(toadd);
                            else
                            {
                                existing.Value = ((existing.Value * existing.Count) + (toadd.Value * toadd.Count)) / (existing.Count + toadd.Count);
                                existing.Count += toadd.Count;
                            }
                        }
                        return db;
                    });
                    if (!success)
                        _logger.Warn("Failed to reduce {0} metrics into buckets {1}h", levelTwo.Count(), levelTwoBucket.TotalHours);
                }



            }

        }

        ///
        /// Storing in riak
        /// Each new metric
        ///   1. Pull riak for quantum ("cpu_usage.16903330000.5s")
        ///   2. Update quantum with average / min / max etc
        ///   3. Save
        ///   4. Pull 5s quantums older than 1 hour
        ///   5. Merge into 5 minute quantums
        ///   6. Delete old 5s quantums, save new 5 minute quantums
        ///   7. Pull 5m quantums older than 1 day
        ///   8. Merge into 1 hour quantums
        ///   9. Delete 5m quantums, save new 1 hour quantums
        ///   10. Same for week, month, and finally year
        /// 
        ///   
        ///

        private async Task<RiakObject> CreateNewDb(string streamId)
        {
            var id = new RiakObjectId("default", "Timeseries", streamId);

            var obj = new RiakObject(id, new Quantum[] { });

            var options = new RiakPutOptions { IfNoneMatch = true, IfNotModified = false, ReturnHead = true, ReturnBody = true };
            options.SetW(Quorum.WellKnown.Quorum);
            _logger.Debug("Saving new timeseries db with id {0}", id.Key);
            var result = await _client.Async.Put(obj, options).ConfigureAwait(false);

            if (!result.IsSuccess && result.ErrorMessage != "modified")
            {
                _logger.Warn("Failed to create new ts db for id {0}.  Error: {1}", id.Key, result.ErrorMessage);
                return null;
            }

            var goptions = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
            goptions.SetRw(Quorum.WellKnown.Quorum);
            _logger.Debug("Getting timeseries db of id {0}", id.Key);
            result = await _client.Async.Get(id, goptions).ConfigureAwait(false);

            return result.Value;
        }

        private async Task<bool> UpdateTimeDb(string streamId, Func<IEnumerable<Quantum>,IEnumerable<Quantum>> updateAction)
        {
            var retries = 0;
            while (retries < 5)
            {
                var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
                options.SetRw(Quorum.WellKnown.Quorum);

                var id = new RiakObjectId("default", "Timeseries", streamId);
                _logger.Debug("Getting timeseries db of id {0}", id.Key);
                var result = await _client.Async.Get(id, options).ConfigureAwait(false);

                RiakObject timedb = result.Value;
                if (!result.IsSuccess)
                {
                    timedb = await CreateNewDb(streamId).ConfigureAwait(false);
                }
                if(timedb == null)
                {
                    _logger.Error("Could not find or create ts db for metric {0}", streamId);
                    continue;
                }

                var quantums = timedb.GetObject<IEnumerable<Quantum>>();
                quantums = updateAction(quantums);

                timedb.SetObject(quantums);

                var uptOptions = new RiakPutOptions { IfNoneMatch = false, IfNotModified = true, ReturnHead = true, ReturnBody = false };
                uptOptions.SetW(Quorum.WellKnown.Quorum);
                var updated = await _client.Async.Put(timedb, uptOptions).ConfigureAwait(false);

                if (updated.IsSuccess)
                    return true;
                retries++;
            }
            return false;
        }

        public async Task<bool> SaveMetric(string streamId, string name, object value, long timestamp, IDictionary<string, string> tags)
        {
            var quantum = new Quantum
            {
                Name = name,
                Tags = tags ?? new Dictionary<string, string>(),
                Timestamp = timestamp,
                Duration = "x"
            };


            var success = await UpdateTimeDb(streamId, (timedb) =>
            {
                timedb = timedb.TryAdd(quantum, x => x.Id);
                return timedb;
            }).ConfigureAwait(false);

            if (!success)
            {
                _logger.Error("Failed to add new quantum id {0} to timedb", quantum.Id);
                return false;
            }
            _updatedNames.Add(streamId);

            return true;
        }

        public async Task<bool> SaveMetrics(string streamId, IEnumerable<Datapoint> metrics)
        {
            var quantums = metrics.Select(x => new Quantum
            {
                Name = x.Name,
                Tags = x.Tags ?? new Dictionary<string, string>(),
                Timestamp = x.Timestamp,
                Duration = "x"
            });
            
            var success = await UpdateTimeDb(streamId, (timedb) =>
            {
                quantums.ForEach(x => timedb = timedb.TryAdd(x, y => y.Id));
                return timedb;
            }).ConfigureAwait(false);
            if(!success)
                _logger.Error("Failed to add new quantums to timedb {0}", streamId);
            _updatedNames.Add(streamId);

            return success;
        }


        public async Task<IEnumerable<Datapoint>> Retrieve(string streamId, long? @from = null, long? to = null)
        {
            if (!@from.HasValue)
                @from = long.MinValue;
            if (!to.HasValue)
                to = long.MaxValue;

            var options = new RiakGetOptions { BasicQuorum = true, NotFoundOk = false };
            options.SetRw(Quorum.WellKnown.Quorum);

            var id = new RiakObjectId("default", "Timeseries", streamId);

            _logger.Debug("Getting timeseries db of id {0}", id.Key);
            var result = await _client.Async.Get(id, options).ConfigureAwait(false);

            if (!result.IsSuccess)
                return new Datapoint[] { };

            var timeDb = result.Value.GetObject<IEnumerable<Quantum>>();

            var ret = new List<Datapoint>();
            foreach (var timeKey in timeDb.Where(x => x.Timestamp >= @from && x.Timestamp < to))
            {
                var dpId = new RiakObjectId("default", "Timeseries", timeKey.Id);
                _logger.Debug("Getting datapoint of id {0}", dpId.Key);
                var datapoint = await _client.Async.Get(id, options).ConfigureAwait(false);

                if (!datapoint.IsSuccess)
                {
                    _logger.Warn($"Failed to find datapoint of id {dpId.Key}.  Error: {datapoint.ErrorMessage}");
                    continue;
                }
                ret.Add(datapoint.Value.GetObject<Datapoint>());
            }

            return ret;
        }
    }
}
