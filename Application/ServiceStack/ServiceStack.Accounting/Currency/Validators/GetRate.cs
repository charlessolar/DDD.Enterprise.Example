using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class GetRate : AbstractValidator<Services.GetRate>
    {
        public GetRate()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}