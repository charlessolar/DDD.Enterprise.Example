using Demo.Presentation.Authentication.Models.Users;
using ServiceStack.FluentValidation;

namespace Demo.Presentation.Authentication.Users.Validators
{
    public class GetValidator : AbstractValidator<Get>
    {
        public GetValidator()
        {
        }
    }
}