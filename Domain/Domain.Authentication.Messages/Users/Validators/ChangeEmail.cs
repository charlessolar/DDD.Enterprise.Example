using FluentValidation;

namespace Demo.Domain.Authentication.Users.Validators
{
    public class ChangeEmail : AbstractValidator<Commands.ChangeEmail>
    {
        public ChangeEmail()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty().EmailAddress();
        }
    }
}