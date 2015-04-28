using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class ChangeAccuracy : AbstractValidator<Commands.ChangeAccuracy>
    {
        public ChangeAccuracy()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.ComputationalAccuracy).GreaterThanOrEqualTo(0);
        }
    }
}
