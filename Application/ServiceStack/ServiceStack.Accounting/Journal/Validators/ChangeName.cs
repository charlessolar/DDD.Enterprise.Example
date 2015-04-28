using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Validators
{
    public class ChangeName : AbstractValidator<Services.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}