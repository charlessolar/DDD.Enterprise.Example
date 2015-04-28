using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Validators
{
    public class ChangeParent : AbstractValidator<Commands.ChangeParent>
    {
        public ChangeParent()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.ParentId).NotEmpty();
        }
    }
}
