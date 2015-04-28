using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Entities.Period.Validators
{
    public class Start : AbstractValidator<Services.Start>
    {
        public Start()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.PeriodId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
        }
    }
}