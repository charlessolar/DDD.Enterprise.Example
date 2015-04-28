using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Account.Validators
{
    public class Freeze : AbstractValidator<Services.Freeze>
    {
        public Freeze()
        {
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}