using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Country.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.CountryId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(2, 8);
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}