using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Validators
{
    public class End : AbstractValidator<Services.End>
    {
        public End()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
        }
    }
}