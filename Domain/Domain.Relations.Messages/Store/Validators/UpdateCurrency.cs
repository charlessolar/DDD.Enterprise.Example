using FluentValidation;
using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Relations.Store.Validators
{
    public class UpdateCurrency : AbstractValidator<Commands.UpdateCurrency>
    {
        public UpdateCurrency()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}