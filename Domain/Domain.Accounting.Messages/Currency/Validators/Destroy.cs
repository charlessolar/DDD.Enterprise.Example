using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class Destroy : AbstractValidator<Commands.Destroy>
    {
        public Destroy()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}