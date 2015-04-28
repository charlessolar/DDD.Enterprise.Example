using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Entry.Validators
{
    public class RequestReview : AbstractValidator<Services.RequestReview>
    {
        public RequestReview()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.EntryId).NotEmpty();
        }
    }
}