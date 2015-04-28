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
    public class UpdateDescription : AbstractValidator<Services.UpdateDescription>
    {
        public UpdateDescription()
        {
            RuleFor(x => x.WarehouseId).NotEmpty();
        }
    }
}