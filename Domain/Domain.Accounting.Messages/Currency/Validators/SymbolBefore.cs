using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class SymbolBefore : AbstractValidator<Commands.SymbolBefore>
    {
        public SymbolBefore()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}
