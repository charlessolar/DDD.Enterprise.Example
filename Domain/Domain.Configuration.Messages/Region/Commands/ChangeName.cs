using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Commands
{
    public class ChangeName : DemoCommand
    {
        public Guid RegionId { get; set; }

        public String Name { get; set; }
    }
}