using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class ChangeAccount : AbstractValidator<Services.ChangeAccount>
    {
        public ChangeAccount()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.AccountId).NotEmpty();
        }
    }
}