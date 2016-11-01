using FluentValidation;

namespace Demo.Domain.Authentication.Users.Validators
{
    public class ChangeName : AbstractValidator<Commands.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}