using FluentValidation;
using Demo.Library.Queries.Validation;

namespace Demo.Application.RavenDB.Inventory.SerialNumbers.Validators
{
    public class Get : BasicQueryValidator<Queries.Get>
    {
        public Get()
            : base()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}