using Demo.Domain.Configuration.Region.Entities.Match;
using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Entities.Match.Services
{
    [Api("Configuration")]
    [Route("/configuration/region/{RegionId}/match", "POST")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid RegionId { get; set; }

        public Guid MatchId { get; set; }

        public String Value { get; set; }

        public MATCH_TYPE Type { get; set; }

        public MATCH_OPERATION Operation { get; set; }
    }
}