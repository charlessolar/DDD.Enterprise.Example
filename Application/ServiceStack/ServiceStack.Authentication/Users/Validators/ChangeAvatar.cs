
using Demo.Application.ServiceStack.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Authentication.Users.Validators
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