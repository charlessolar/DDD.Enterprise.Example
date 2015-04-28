using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Entry.Validators
{
    public class Reviewed : AbstractValidator<Services.Reviewed>
    {
        public Reviewed()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.EntryId).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}