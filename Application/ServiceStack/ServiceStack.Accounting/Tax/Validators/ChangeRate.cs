using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class ChangeRate : AbstractValidator<Services.ChangeRate>
    {
        public ChangeRate()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.Rate).GreaterThan(0);
        }
    }
}