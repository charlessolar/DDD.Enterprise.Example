using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Inventory.Items.Validators
{
    public class ChangeDescription : AbstractValidator<Commands.ChangeDescription>
    {
        public ChangeDescription()
        {
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}