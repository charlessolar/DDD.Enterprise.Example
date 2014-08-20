
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items.Validators
{
    public class GetItemValidator : AbstractValidator<GetItem>
    {
        public GetItemValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
