using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Validators
{
    public class ChangeCheckDate : AbstractValidator<Services.ChangeCheckDate>
    {
        public ChangeCheckDate()
        {
            RuleFor(x => x.JournalId).NotEmpty();
        }
    }
}