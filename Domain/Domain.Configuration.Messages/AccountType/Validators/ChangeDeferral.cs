using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType.Validators
{
    public class ChangeDeferral : AbstractValidator<Commands.ChangeDeferral>
    {
        public ChangeDeferral()
        {
            RuleFor(x => x.AccountTypeId).NotEmpty();
            RuleFor(x => x.DeferralMethod).NotNull();
        }
    }
}