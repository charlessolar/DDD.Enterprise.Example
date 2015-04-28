using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store.Validators
{
    public class UpdateName : AbstractValidator<Services.UpdateName>
    {
        public UpdateName()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}