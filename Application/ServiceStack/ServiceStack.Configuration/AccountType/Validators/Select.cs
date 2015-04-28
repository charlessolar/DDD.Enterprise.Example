using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.AccountType.Validators
{
    public class Select : AbstractValidator<Services.Select>
    {
        public Select()
        {
            RuleFor(x => x.Id).NotEqual(Guid.Empty);
        }
    }
}