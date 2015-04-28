using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Entities.Match.Validators
{
    public class Index : AbstractValidator<Services.Index>
    {
        public Guid RegionId { get; set; }
    }
}