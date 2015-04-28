using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class ChangeFormat : AbstractValidator<Services.ChangeFormat>
    {
        public ChangeFormat()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Format).NotEmpty();
        }
    }
}