using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class ChangeDescription : AbstractValidator<Services.ChangeDescription>
    {
        public ChangeDescription()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}