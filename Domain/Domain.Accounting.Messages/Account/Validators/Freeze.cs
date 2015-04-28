using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Validators
{
    public class Freeze : AbstractValidator<Commands.Freeze>
    {
        public Freeze()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}
