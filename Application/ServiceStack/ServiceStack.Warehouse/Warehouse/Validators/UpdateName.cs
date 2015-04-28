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
    public class UpdateName : AbstractValidator<Services.UpdateName>
    {
        public UpdateName()
        {
            RuleFor(x => x.WarehouseId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}