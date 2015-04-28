using Demo.Library.Command;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Validators
{
    public class Create : AbstractValidator<Services.Create>
    {
        public Create()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.Identity).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}