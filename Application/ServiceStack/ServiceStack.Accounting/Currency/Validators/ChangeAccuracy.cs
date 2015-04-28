using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class ChangeAccuracy : AbstractValidator<Services.ChangeAccuracy>
    {
        public ChangeAccuracy()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Accuracy).GreaterThanOrEqualTo(0);
        }
    }
}