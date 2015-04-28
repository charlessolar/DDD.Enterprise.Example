using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Tax.Validators
{
    public class Deactivate : AbstractValidator<Services.Deactivate>
    {
        public Deactivate()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}