using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class ChangeName : AbstractValidator<Services.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}