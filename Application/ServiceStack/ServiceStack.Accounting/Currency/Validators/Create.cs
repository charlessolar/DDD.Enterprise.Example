using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Currency.Validators
{
    public class Create : AbstractValidator<Services.Create>
    {
        public Create()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(2, 8);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Symbol).NotEmpty().Length(1);
            RuleFor(x => x.RoundingFactor).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ComputationalAccuracy).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Format).NotEmpty();
            RuleFor(x => x.Fraction).NotEmpty();
        }
    }
}