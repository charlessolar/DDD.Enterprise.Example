using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Validators
{
    public class RemoveWarehouse : AbstractValidator<Commands.RemoveWarehouse>
    {
        public RemoveWarehouse()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.WarehouseId).NotEmpty();
        }
    }
}