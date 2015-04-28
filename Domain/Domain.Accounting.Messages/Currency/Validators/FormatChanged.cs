using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class ChangeFormat : AbstractValidator<Commands.ChangeFormat>
    {
        public ChangeFormat()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Format).NotEmpty();
        }
    }
}