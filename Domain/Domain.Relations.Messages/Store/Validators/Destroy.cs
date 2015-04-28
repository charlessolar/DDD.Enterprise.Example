using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Relations.Store.Validators
{
    public class Destroy : AbstractValidator<Commands.Destroy>
    {
        public Destroy()
        {
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}