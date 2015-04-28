using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Validators
{
    public class AddWarehouse : AbstractValidator<Commands.AddWarehouse>
    {
        public AddWarehouse()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.WarehouseId).NotEmpty();
        }
    }
}