using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Commands
{
    public class Destroy : DemoCommand
    {
        public Guid RegionId { get; set; }

        public Guid MatchId { get; set; }
    }
}