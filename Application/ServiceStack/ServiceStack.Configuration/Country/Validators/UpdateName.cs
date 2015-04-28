using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.Country.Validators
{
    public class UpdateName : AbstractValidator<Services.UpdateName>
    {
        public UpdateName()
        {
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
