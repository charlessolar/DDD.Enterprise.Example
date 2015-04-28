using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Entities.Match.Validators
{
    public class ChangeOperation : AbstractValidator<Services.ChangeOperation>
    {
        public Guid RegionId { get; set; }

        public Guid MatchId { get; set; }

        public String Operation { get; set; }
    }
}