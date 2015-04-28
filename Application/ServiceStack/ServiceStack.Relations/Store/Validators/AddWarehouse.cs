using Demo.Library.Command;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Validators
{
    public class AddWarehouse : AbstractValidator<Services.AddWarehouse>
    {
        public AddWarehouse()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.WarehouseId).NotEmpty();
        }
    }
}