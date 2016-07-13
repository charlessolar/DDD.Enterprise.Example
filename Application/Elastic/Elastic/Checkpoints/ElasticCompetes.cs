using Aggregates;
using Aggregates.Exceptions;
using log4net;
using Nest;
using NLog;
using Demo.Application.Elastic.Infrastructure;
using Demo.Library.Checkpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Elastic.Checkpoints
{
    public class ElasticCompetes : IManageCompetes
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static Guid Discriminator = Guid.NewGuid();
        private readonly IElasticClient _client;
        public ElasticCompetes(IElasticClient client)
        {
            _client = client;
        }

        public bool CheckOrSave(string endpoint, Int32 bucket, long position)
        {
            //Logger.Debug("Endpoint {0} attempting to claim bucket {1} at position {2}", endpoint, bucket, position);
            if (!_client.DocumentExists<Competer>($"{endpoint}:{bucket}").Exists)
            {
                _client.Index(new Competer
                {
                    Id = $"{endpoint}:{bucket}",
                    Discriminator = Discriminator,
                    Endpoint = endpoint,
                    Bucket = bucket,
                    Heartbeat = DateTime.UtcNow,
                    Position = position
                }, x => x.Consistency(Elasticsearch.Net.Consistency.All));

                Thread.Sleep(new Random().Next(100, 2000));

                var check = _client.Get<Competer>($"{endpoint}:{bucket}");
                if( check.Source?.Discriminator == Discriminator)
                    Logger.Info("Endpoint {0} successfully claimed bucket {1}", endpoint, bucket);
                else
                    Logger.Info("Endpoint {0} failed to claim bucket {1}.  Error: {2}", endpoint, bucket, "Discriminator mismatch");
                return check.Source?.Discriminator == Discriminator;
            }
            return false;
        }


        public DateTime? LastHeartbeat(string endpoint, Int32 bucket)
        {
            var doc = _client.Get<Competer>($"{endpoint}:{bucket}");

            if (!doc.Found) return null;
            return doc.Source.Heartbeat;
        }

        public long LastPosition(string endpoint, Int32 bucket)
        {
            var doc = _client.Get<Competer>($"{endpoint}:{bucket}");
            if (!doc.Found) return 0;
            return doc.Source.Position;
        }

        public void Heartbeat(string endpoint, Int32 bucket, DateTime Timestamp, long? position)
        {
            var doc = _client.Get<Competer>($"{endpoint}:{bucket}");

            if (doc?.Source.Discriminator != Discriminator)
                throw new DiscriminatorException();

            if(position.HasValue)
                _client.Update<Competer, Object>($"{endpoint}:{bucket}", x => x
                    .Doc(new { Heartbeat = Timestamp, Position = position })
                    .Consistency(Elasticsearch.Net.Consistency.All));
            else
                _client.Update<Competer, Object>($"{endpoint}:{bucket}", x => x
                    .Doc(new { Heartbeat = Timestamp })
                    .Consistency(Elasticsearch.Net.Consistency.All));

        }
        public Boolean Adopt(String endpoint, Int32 bucket, DateTime Timestamp)
        {
            Logger.Info("Endpoint {0} attempting to adopt bucket {1}", endpoint, bucket);
            var response = _client.Update<Competer, Object>($"{endpoint}:{bucket}", x => x
                .Doc(new { Discriminator = Discriminator, Heartbeat = Timestamp })
                .Consistency(Elasticsearch.Net.Consistency.All));

            Thread.Sleep(new Random().Next(100, 2000));

            var check = _client.Get<Competer>($"{endpoint}:{bucket}");
            if (check.Source?.Discriminator == Discriminator)
                Logger.Info("Endpoint {0} successfully adopted bucket {1}", endpoint, bucket);
            else
                Logger.Info("Endpoint {0} failed to adopt bucket {1}.  Error: {2}", endpoint, bucket, "Discriminator mismatch");
            return check.Source?.Discriminator == Discriminator;
        }
    }
}
