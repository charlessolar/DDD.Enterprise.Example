using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class AddRate : AbstractValidator<Services.AddRate>
    {
        public AddRate()
        {
            RuleFor(x => x.RateId).NotEmpty();
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.DestinationCurrencyId).NotEmpty();
            RuleFor(x => x.Factor).GreaterThan(0);
        }
    }
}