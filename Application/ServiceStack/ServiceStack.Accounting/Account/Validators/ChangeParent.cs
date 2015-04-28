using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Account.Validators
{
    public class ChangeParent : AbstractValidator<Services.ChangeParent>
    {
        public ChangeParent()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.ParentId).NotEqual(Guid.Empty);
        }
    }
}