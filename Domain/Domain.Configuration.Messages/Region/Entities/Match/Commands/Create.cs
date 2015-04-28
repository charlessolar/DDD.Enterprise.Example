using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Commands
{
    public class Create : DemoCommand
    {
        public Guid RegionId { get; set; }

        public Guid MatchId { get; set; }

        public String Value { get; set; }

        public MATCH_TYPE Type { get; set; }

        public MATCH_OPERATION Operation { get; set; }
    }
}