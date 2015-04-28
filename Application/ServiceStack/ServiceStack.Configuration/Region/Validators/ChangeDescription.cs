using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Region.Validators
{
    public class ChangeDescription : AbstractValidator<Services.ChangeDescription>
    {
        public ChangeDescription()
        {
            RuleFor(x => x.RegionId).NotEmpty();
        }
    }
}