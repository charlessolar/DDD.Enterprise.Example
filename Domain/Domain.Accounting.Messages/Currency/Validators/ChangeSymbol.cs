using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class ChangeSymbol : AbstractValidator<Commands.ChangeSymbol>
    {
        public ChangeSymbol()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Symbol).NotEmpty().Length(1);
        }
    }
}
