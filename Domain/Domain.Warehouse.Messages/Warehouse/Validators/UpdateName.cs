using FluentValidation;
using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Warehouse.Warehouse.Validators
{
    public class UpdateName : AbstractValidator<Commands.UpdateName>
    {
        public UpdateName()
        {
            RuleFor(x => x.WarehouseId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}