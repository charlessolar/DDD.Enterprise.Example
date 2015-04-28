using NServiceBus;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Events
{
    public interface Created : IEvent
    {
        Guid RegionId { get; set; }

        Guid MatchId { get; set; }

        String Value { get; set; }

        MATCH_TYPE Type { get; set; }

        MATCH_OPERATION Operation { get; set; }
    }
}