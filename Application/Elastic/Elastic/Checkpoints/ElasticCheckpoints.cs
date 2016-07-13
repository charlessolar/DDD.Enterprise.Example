using Aggregates;
using Nest;
using Demo.Application.Elastic.Infrastructure;
using Demo.Library.Checkpoints;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.Elastic.Checkpoints
{
    public class ElasticCheckpoints : IPersistCheckpoints
    {
        private readonly IElasticClient _client;

        public ElasticCheckpoints(IElasticClient client)
        {
            _client = client;
        }

        public async Task<EventStore.ClientAPI.Position> Load(String endpoint)
        {
            var position = await _client.GetAsync<Checkpoint>(endpoint);
            if (!position.Found) return EventStore.ClientAPI.Position.Start;
            return new EventStore.ClientAPI.Position(position.Source.Position, position.Source.Position);
        }

        public async Task Save(String endpoint, long position)
        {
            await _client.UpdateAsync<Checkpoint, Object>(endpoint, x => x
                .Doc(new { Position = position })
                .Upsert(new Checkpoint { Id = endpoint, Position = position }));
        }

    }
}
