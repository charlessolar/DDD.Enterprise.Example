using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(1, 8);
            RuleFor(x => x.Name).NotEmpty().Length(2);
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Operation).NotNull();
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}