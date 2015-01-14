using FluentValidation;

namespace Demo.Domain.Authentication.Users.Validators
{
    public class Logout : AbstractValidator<Commands.Logout>
    {
        public Logout()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}