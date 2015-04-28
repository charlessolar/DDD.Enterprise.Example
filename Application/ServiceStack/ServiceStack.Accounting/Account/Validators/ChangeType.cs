using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Account.Validators
{
    public class ChangeType : AbstractValidator<Services.ChangeType>
    {
        public ChangeType()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.TypeId).NotEqual(Guid.Empty);
        }
    }
}