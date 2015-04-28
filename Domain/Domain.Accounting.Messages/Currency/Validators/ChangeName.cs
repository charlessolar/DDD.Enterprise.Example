using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class ChangeName : AbstractValidator<Commands.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
