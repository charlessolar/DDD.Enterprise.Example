using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items.Validators
{
    public class CreateItem : AbstractValidator<Models.CreateItem>
    {
        public CreateItem()
        {
            RuleFor(x => x.Number).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.UnitOfMeasure).NotEmpty().Length(2, 8).WithMessage("Unit of Measure must be between 2 and 8 characters");
            RuleFor(x => x.CatalogPrice).GreaterThan(0.0M);
            RuleFor(x => x.CostPrice).GreaterThan(0.0M);
        }
    }
}