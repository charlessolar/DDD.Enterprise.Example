using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Commands
{
    public class Create : DemoCommand
    {
        public Guid RegionId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}