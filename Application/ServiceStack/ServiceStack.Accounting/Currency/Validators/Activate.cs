using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class Activate : AbstractValidator<Services.Activate>
    {
        public Activate()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}