using FluentValidation;
using Demo.Library.Queries.Validation;

namespace Demo.Application.RavenDB.Inventory.Items.Validators
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