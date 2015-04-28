using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.Identity).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}