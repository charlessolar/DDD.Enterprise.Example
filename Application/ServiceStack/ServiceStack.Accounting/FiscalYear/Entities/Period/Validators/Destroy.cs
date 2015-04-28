using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Entities.Period.Validators
{
    public class Destroy : AbstractValidator<Services.Destroy>
    {
        public Destroy()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.PeriodId).NotEmpty();
        }
    }
}