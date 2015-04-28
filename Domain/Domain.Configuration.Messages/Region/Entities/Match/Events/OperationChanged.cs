using NServiceBus;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Events
{
    public interface OperationChanged : IEvent
    {
        Guid RegionId { get; set; }

        Guid MatchId { get; set; }

        MATCH_OPERATION Operation { get; set; }
    }
}