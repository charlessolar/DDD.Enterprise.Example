using Demo.Library.Command;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Validators
{
    public class RemoveWarehouse : AbstractValidator<Services.RemoveWarehouse>
    {
        public RemoveWarehouse()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.WarehouseId).NotEmpty();
        }
    }
}