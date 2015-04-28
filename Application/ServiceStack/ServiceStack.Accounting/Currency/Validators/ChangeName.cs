using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class ChangeName : AbstractValidator<Services.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}