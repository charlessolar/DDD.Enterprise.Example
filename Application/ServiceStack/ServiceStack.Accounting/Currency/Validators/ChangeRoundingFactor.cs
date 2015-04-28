using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class ChangeRoundingFactor : AbstractValidator<Services.ChangeRoundingFactor>
    {
        public ChangeRoundingFactor()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.RoundingFactor).GreaterThanOrEqualTo(0);
        }
    }
}