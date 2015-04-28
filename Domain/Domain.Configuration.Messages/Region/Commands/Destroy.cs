using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Commands
{
    public class Destroy : DemoCommand
    {
        public Guid RegionId { get; set; }
    }
}