using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class ChangeSymbol : AbstractValidator<Services.ChangeSymbol>
    {
        public ChangeSymbol()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Symbol).NotEmpty().Length(1);
        }
    }
}