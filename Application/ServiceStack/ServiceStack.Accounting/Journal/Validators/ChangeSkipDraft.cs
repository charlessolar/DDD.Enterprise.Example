using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Validators
{
    public class ChangeSkipDraft : AbstractValidator<Services.ChangeSkipDraft>
    {
        public ChangeSkipDraft()
        {
            RuleFor(x => x.JournalId).NotEmpty();
        }
    }
}