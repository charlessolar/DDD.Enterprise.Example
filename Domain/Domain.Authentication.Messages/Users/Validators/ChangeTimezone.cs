using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Authentication.Users.Validators
{
    public class ChangeTimezone : AbstractValidator<Commands.ChangeTimezone>
    {
        public ChangeTimezone()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Timezone).NotEmpty();
        }
    }
}