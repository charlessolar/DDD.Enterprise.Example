using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Aggregates;
using Demo.Library.Checkpoints;
using Demo.Application.Riak.Infrastructure;
using Demo.Application.Riak.Infrastructure.Riak;
using Demo.Application.Riak.Infrastructure.Exceptions;
using RiakClient.Models;
using System.Threading.Tasks;
using RiakClient;

namespace Demo.Application.Riak.Checkpoints
{
    public class RiakCheckpoints : IPersistCheckpoints
    {
        private readonly IRiakClient _riak;

        public RiakCheckpoints(IRiakClient riak)
        {
            _riak = riak;
        }

        public async Task<EventStore.ClientAPI.Position> Load(String endpoint)
        {
            var id = new RiakObjectId($"{Settings.Bucket}.system", endpoint);

            var options = new RiakGetOptions { };
            options.SetRw(Quorum.WellKnown.All);
            var exists = await _riak.Async.Get(id, options);
            if (exists.IsSuccess)
            {
                var position = exists.Value.GetObject<Checkpoint>();
                return new EventStore.ClientAPI.Position(position.Position, position.Position);
            }

            return EventStore.ClientAPI.Position.Start;
        }

        public async Task Save(String endpoint, long position)
        {
            var id = new RiakObjectId($"{Settings.Bucket}.system", endpoint);

            var options = new RiakGetOptions { };
            options.SetRw(Quorum.WellKnown.All);
            var exists = await _riak.Async.Get(id, options);
            if (exists.IsSuccess)
            {
                var checkpoint = exists.Value.GetObject<Checkpoint>();
                checkpoint.Position = position;

                exists.Value.SetObject(checkpoint);

                var putOpt = new RiakPutOptions { IfNotModified = true, IfNoneMatch = false };
                putOpt.SetW(Quorum.WellKnown.All);
                await _riak.Async.Put(exists.Value, putOpt);
            }
            else
            {
                var checkpoint = new Checkpoint
                {
                    Id = endpoint,
                    Position = position,
                };
                var putOpt = new RiakPutOptions { IfNotModified = false, IfNoneMatch = true };
                putOpt.SetW(Quorum.WellKnown.All);
                var o = new RiakObject(id, checkpoint);
                await _riak.Async.Put(o, putOpt);

            }
        }
    }
}