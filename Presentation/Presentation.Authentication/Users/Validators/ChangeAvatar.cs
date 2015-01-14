using Demo.Presentation.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Authentication.Users.Validators
{
    public class ChangeAvatarValidator : AbstractValidator<ChangeAvatar>
    {
        public ChangeAvatarValidator()
        {
            RuleFor(x => x.ImageType).NotEmpty();
            RuleFor(x => x.ImageData).NotEmpty();
        }
    }
}