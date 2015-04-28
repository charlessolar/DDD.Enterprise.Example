using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Commands
{
    public class ChangeDescription : DemoCommand
    {
        public Guid RegionId { get; set; }

        public String Description { get; set; }
    }
}