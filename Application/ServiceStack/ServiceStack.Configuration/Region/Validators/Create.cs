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
    public class Create : AbstractValidator<Services.Create>
    {
        public Create()
        {
            RuleFor(x => x.RegionId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(2, 8);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ParentId).NotEqual(Guid.Empty);
        }
    }
}