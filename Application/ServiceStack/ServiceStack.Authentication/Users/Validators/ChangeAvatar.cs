
using Forte.Application.ServiceStack.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Forte.Application.ServiceStack.Authentication.Users.Validators
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