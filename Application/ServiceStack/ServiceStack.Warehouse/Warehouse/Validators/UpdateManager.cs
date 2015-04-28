using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Warehouse.Warehouse.Validators
{
    public class UpdateManager : AbstractValidator<Services.UpdateManager>
    {
        public UpdateManager()
        {
            RuleFor(x => x.WarehouseId).NotEmpty();
            RuleFor(x => x.EmployeeId).NotEqual(Guid.Empty);
        }
    }
}