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
    [Route("/configuration/region/{RegionId}/description", "PUT POST")]
    public class ChangeDescription : IReturn<Base<Command>>
    {
        public Guid RegionId { get; set; }

        public String Description { get; set; }
    }
}