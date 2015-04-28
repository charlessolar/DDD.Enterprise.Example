using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Validators
{
    public class ChangeName : AbstractValidator<Services.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}