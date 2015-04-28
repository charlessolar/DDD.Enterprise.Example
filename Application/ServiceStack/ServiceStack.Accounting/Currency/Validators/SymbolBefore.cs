using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class SymbolBefore : AbstractValidator<Services.SymbolBefore>
    {
        public SymbolBefore()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
        }
    }
}