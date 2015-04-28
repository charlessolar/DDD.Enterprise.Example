using Demo.Library.Queries;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Entities.Match.Services
{
    [Api("Configuration")]
    [Route("/configuration/region/{RegionId}/match", "GET")]
    public class Index : PagedQuery<Responses.Index>
    {
        public Guid RegionId { get; set; }
    }
}