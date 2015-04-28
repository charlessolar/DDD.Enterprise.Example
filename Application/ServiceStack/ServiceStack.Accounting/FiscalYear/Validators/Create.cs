using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.FiscalYear.Validators
{
    public class Create : AbstractValidator<Services.Create>
    {
        public Create()
        {
            RuleFor(x => x.FiscalYearId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(2, 8);
        }
    }
}