using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.AccountTypeId).NotEmpty();
            RuleFor(x => x.DeferralMethod).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}