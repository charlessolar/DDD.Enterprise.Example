using NServiceBus;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Events
{
    public interface ValueChanged : IEvent
    {
        Guid RegionId { get; set; }

        Guid MatchId { get; set; }

        String Value { get; set; }
    }
}