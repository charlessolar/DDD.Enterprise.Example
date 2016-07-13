using Aggregates;
using Aggregates.Exceptions;
using log4net;
using Newtonsoft.Json;
using NLog;
using Demo.Application.Riak.Infrastructure.Exceptions;
using Demo.Application.Riak.Infrastructure.Riak;
using Demo.Library.Checkpoints;
using RiakClient;
using RiakClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Application.Riak.Checkpoints
{
    public class RiakCompetes : IManageCompetes
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private static Guid Discriminator = Guid.NewGuid();
        private readonly IRiakClient _riak;
        public RiakCompetes(IRiakClient riak)
        {
            _riak = riak;
        }

        public bool CheckOrSave(string endpoint, Int32 bucket, long position)
        {
            var id = new RiakObjectId($"{Settings.Bucket}.system", $"{endpoint}:{bucket}");

            var options = new RiakGetOptions { };
            options.SetRw(Quorum.WellKnown.All);
            var exists = _riak.Get(id, options);

            if (exists.IsSuccess) return false;

            var competer = new Competer
            {
                Id = $"{endpoint}:{bucket}",
                Discriminator = Discriminator,
                Endpoint = endpoint,
                Bucket = bucket,
                Position = position,
                Heartbeat = DateTime.UtcNow
            };

            var putOpt = new RiakPutOptions { IfNotModified = false, IfNoneMatch = true };
            putOpt.SetW(Quorum.WellKnown.All);
            var o = new RiakObject(id, competer);
            var result = _riak.Put(o, putOpt);
            if (!result.IsSuccess)
            {
                Logger.Info("Endpoint {0} failed to claim bucket {1}.  Error: {2}", endpoint, bucket, result.ErrorMessage);
                return false;
            }

            Thread.Sleep(new Random().Next(100, 2000));

            do
            {
                exists = _riak.Get(id, options);
            } while (!exists.IsSuccess);

            competer = exists.Value.GetObject<Competer>();
            if (competer.Discriminator != Discriminator)
            {
                Logger.Info("Endpoint {0} failed to claim bucket {1}.  Error: {2}", endpoint, bucket, "Discriminator mismatch");
                return false;
            }
            Logger.Info("Endpoint {0} successfully claimed bucket {1}", endpoint, bucket);
            return true;

        }


        public DateTime? LastHeartbeat(string endpoint, Int32 bucket)
        {
            var id = new RiakObjectId($"{Settings.Bucket}.system", $"{endpoint}:{bucket}");

            var options = new RiakGetOptions { };
            options.SetRw(Quorum.WellKnown.All);
            var exists = _riak.Get(id, options);

            if (exists.IsSuccess)
            {
                var competer = exists.Value.GetObject<Competer>();
                return competer.Heartbeat;
            }
            return null;
        }

        public long LastPosition(string endpoint, Int32 bucket)
        {
            var id = new RiakObjectId($"{Settings.Bucket}.system", $"{endpoint}:{bucket}");

            var options = new RiakGetOptions { };
            options.SetRw(Quorum.WellKnown.All);
            var exists = _riak.Get(id, options);

            if (exists.IsSuccess)
            {
                var competer = exists.Value.GetObject<Competer>();
                return competer.Position;
            }
            return 0;
        }

        public void Heartbeat(string endpoint, Int32 bucket, DateTime Timestamp, long? position)
        {

            var id = new RiakObjectId($"{Settings.Bucket}.system", $"{endpoint}:{bucket}");

            var options = new RiakGetOptions { };
            options.SetRw(Quorum.WellKnown.All);
            var exists = _riak.Get(id, options);

            if (exists.IsSuccess)
            {
                var competer = exists.Value.GetObject<Competer>();

                if (competer.Discriminator != Discriminator)
                    throw new DiscriminatorException();

                competer.Heartbeat = Timestamp;
                competer.Position = position ?? competer.Position;

                exists.Value.SetObject(competer);
                var putOpt = new RiakPutOptions { IfNotModified = true, IfNoneMatch = false };
                putOpt.SetW(Quorum.WellKnown.All);

                _riak.Put(exists.Value, putOpt);
            }

        }
        public Boolean Adopt(String endpoint, Int32 bucket, DateTime Timestamp)
        {
            Logger.Info("Endpoint {0} attempting to adopt bucket {1}", endpoint, bucket);
            var id = new RiakObjectId($"{Settings.Bucket}.system", $"{endpoint}:{bucket}");

            var options = new RiakGetOptions { };
            options.SetRw(Quorum.WellKnown.All);
            var exists = _riak.Get(id, options);
            
            if (!exists.IsSuccess)
            {
                Logger.Error("Endpoint {0} failed to adopt bucket {1}. Error: {2}", endpoint, bucket, exists.ErrorMessage);
                return false;
            }

            var competer = exists.Value.GetObject<Competer>();

            competer.Heartbeat = Timestamp;
            competer.Discriminator = Discriminator;

            exists.Value.SetObject(competer);
            var putOpt = new RiakPutOptions { IfNotModified = true, IfNoneMatch = false };
            putOpt.SetW(Quorum.WellKnown.All);

            var putResult = _riak.Put(exists.Value, putOpt);
            if (!putResult.IsSuccess)
            {
                Logger.Error("Endpoint {0} failed to adopt bucket {1}. Error: {2}", endpoint, bucket, putResult.ErrorMessage);
                return false;
            }

            Thread.Sleep(new Random().Next(100, 2000));

            do
            {
                exists = _riak.Get(id, options);
            } while (!exists.IsSuccess);

            competer = exists.Value.GetObject<Competer>();

            if (competer.Discriminator == Discriminator)
                Logger.Info("Endpoint {0} successfully adopted bucket {1}", endpoint, bucket);
            else
                Logger.Info("Endpoint {0} failed to adopt bucket {1}.  Error: {2}", endpoint, bucket, "Discriminator mismatch");
            return competer.Discriminator == Discriminator;

        }
    }
}
