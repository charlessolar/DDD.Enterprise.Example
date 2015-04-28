using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Services
{
    [Api("Configuration")]
    [Route("/configuration/region", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid RegionId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Guid? ParentId { get; set; }
    }
}