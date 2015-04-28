using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class Activate : AbstractValidator<Services.Activate>
    {
        public Activate()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}