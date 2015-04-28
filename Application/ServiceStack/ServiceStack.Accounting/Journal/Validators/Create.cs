using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Accounting.Journal.Validators
{
    public class Create : AbstractValidator<Services.Create>
    {
        public Create()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(2, 8);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ResponsibleId).NotEmpty();
        }
    }
}