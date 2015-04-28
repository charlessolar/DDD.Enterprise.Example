using Demo.Library.Command;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Validators
{
    public class Destroy : AbstractValidator<Services.Destroy>
    {
        public Destroy()
        {
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}