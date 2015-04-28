using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Validators
{
    public class ChangeType : AbstractValidator<Commands.ChangeType>
    {
        public ChangeType()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.TypeId).NotEqual(Guid.Empty);
        }
    }
}