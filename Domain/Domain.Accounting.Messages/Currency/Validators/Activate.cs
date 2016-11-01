using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class Activate : AbstractValidator<Commands.Activate>
    {
        public Activate()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}
