using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Authentication.Users.Validators
{
    public class ChangeTimezone : AbstractValidator<Services.ChangeTimezone>
    {
        public ChangeTimezone()
        {
            RuleFor(x => x.Timezone).NotEmpty();
        }
    }
}