using NServiceBus;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Events
{
    public interface TypeChanged : IEvent
    {
        Guid RegionId { get; set; }

        Guid MatchId { get; set; }

        MATCH_TYPE Type { get; set; }
    }
}