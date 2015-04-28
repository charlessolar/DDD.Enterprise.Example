using ServiceStack.FluentValidation;
using System;

namespace Demo.Application.ServiceStack.Accounting.Account.Validators
{
    public class Create : AbstractValidator<Services.Create>
    {
        public Create()
        {
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(1, 8);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Operation).NotEmpty();
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}