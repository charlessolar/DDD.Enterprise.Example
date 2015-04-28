using NServiceBus;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Events
{
    public interface Destroyed : IEvent
    {
        Guid RegionId { get; set; }

        Guid MatchId { get; set; }
    }
}