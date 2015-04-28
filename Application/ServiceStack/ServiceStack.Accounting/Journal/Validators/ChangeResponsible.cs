using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Validators
{
    public class ChangeResponsible : AbstractValidator<Services.ChangeResponsible>
    {
        public ChangeResponsible()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}