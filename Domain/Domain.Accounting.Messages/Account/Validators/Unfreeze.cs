using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Validators
{
    public class Unfreeze : AbstractValidator<Commands.Unfreeze>
    {
        public Unfreeze()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}
