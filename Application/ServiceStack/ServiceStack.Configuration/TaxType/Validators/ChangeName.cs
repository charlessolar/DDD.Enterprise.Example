using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.TaxType.Validators
{
    public class ChangeName : AbstractValidator<Services.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.TaxTypeId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}