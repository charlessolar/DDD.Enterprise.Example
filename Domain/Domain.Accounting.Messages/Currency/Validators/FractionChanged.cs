using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class ChangeFraction : AbstractValidator<Commands.ChangeFraction>
    {
        public ChangeFraction()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Fraction).NotEmpty();
        }
    }
}