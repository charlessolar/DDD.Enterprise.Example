using FluentValidation;
using Demo.Library.Queries.Validation;

namespace Demo.Application.RavenDB.Authentication.Users.Validators
{
    internal class Get : BasicQueryValidator<Queries.Get>
    {
        public Get()
            : base()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}