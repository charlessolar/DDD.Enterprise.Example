using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Commands
{
    public class ChangeOperation : DemoCommand
    {
        public Guid RegionId { get; set; }

        public Guid MatchId { get; set; }

        public MATCH_OPERATION Operation { get; set; }
    }
}