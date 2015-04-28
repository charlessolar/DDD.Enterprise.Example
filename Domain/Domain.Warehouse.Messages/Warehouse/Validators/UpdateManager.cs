using FluentValidation;
using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Warehouse.Warehouse.Validators
{
    public class UpdateManager : AbstractValidator<Commands.UpdateManager>
    {
        public UpdateManager()
        {
            RuleFor(x => x.WarehouseId).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEqual(Guid.Empty);
        }
    }
}