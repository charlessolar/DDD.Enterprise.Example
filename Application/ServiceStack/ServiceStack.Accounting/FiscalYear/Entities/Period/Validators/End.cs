using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Entities.Period.Validators
{
    public class End : AbstractValidator<Services.End>
    {
        public End()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.PeriodId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
        }
    }
}