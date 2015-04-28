using FluentValidation;
using Demo.Library.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Relations.Store.Validators
{
    public class UpdateEmail : AbstractValidator<Commands.UpdateEmail>
    {
        public UpdateEmail()
        {
            RuleFor(x => x.StoreId).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}