using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class AddRate : AbstractValidator<Commands.AddRate>
    {
        public AddRate()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Factor).GreaterThanOrEqualTo(0);
            RuleFor(x => x.DestinationCurrencyId).NotEmpty();
        }
    }
}