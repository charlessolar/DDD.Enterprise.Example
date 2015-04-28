using ServiceStack.FluentValidation;

namespace Demo.Application.ServiceStack.Authentication.Users.Validators
{
    public class ChangeAvatar : AbstractValidator<Services.ChangeAvatar>
    {
        public ChangeAvatar()
        {
            RuleFor(x => x.ImageType).NotEmpty();
            RuleFor(x => x.ImageData).NotEmpty();
        }
    }
}