using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class Deactivate : AbstractValidator<Services.Deactivate>
    {
        public Deactivate()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}