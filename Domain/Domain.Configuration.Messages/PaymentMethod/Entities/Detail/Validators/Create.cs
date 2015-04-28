using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.PaymentMethod.Entities.Detail.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.PaymentMethodId).NotEmpty();
            RuleFor(x => x.DetailId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Hint).NotEmpty();
        }
    }
}