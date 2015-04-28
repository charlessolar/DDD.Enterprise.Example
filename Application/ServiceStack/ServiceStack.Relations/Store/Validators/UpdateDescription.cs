using Demo.Library.Command;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;

namespace Demo.Application.ServiceStack.Relations.Store.Validators
{
    public class UpdateDescription : AbstractValidator<Services.UpdateDescription>
    {
        public UpdateDescription()
        {
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}