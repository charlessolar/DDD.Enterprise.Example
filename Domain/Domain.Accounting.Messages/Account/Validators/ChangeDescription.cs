using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Validators
{
    public class ChangeDescription : AbstractValidator<Commands.ChangeDescription>
    {
        public ChangeDescription()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
