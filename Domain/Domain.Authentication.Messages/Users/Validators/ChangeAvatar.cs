using FluentValidation;

namespace Demo.Domain.Authentication.Users.Validators
{
    public class ChangeAvatar : AbstractValidator<Commands.ChangeAvatar>
    {
        public ChangeAvatar()
        {
            RuleFor(x => x.ImageType).NotEmpty();
            RuleFor(x => x.ImageData).NotEmpty();
        }
    }
}