using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class ChangeFraction : AbstractValidator<Services.ChangeFraction>
    {
        public ChangeFraction()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Fraction).NotEmpty();
        }
    }
}