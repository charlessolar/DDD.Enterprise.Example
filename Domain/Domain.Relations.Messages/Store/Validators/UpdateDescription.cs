using FluentValidation;
using System;

namespace Demo.Domain.Relations.Store.Validators
{
    public class UpdateDescription : AbstractValidator<Commands.UpdateDescription>
    {
        public UpdateDescription()
        {
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}