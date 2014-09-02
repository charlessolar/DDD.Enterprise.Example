
using Demo.Presentation.Inventory.Models.Items.Services;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.Inventory.Items.Validators
{
    public class FindItemsValidator : AbstractValidator<FindItems>
    {
        public FindItemsValidator()
            : base()
        {
        }
    }
}
