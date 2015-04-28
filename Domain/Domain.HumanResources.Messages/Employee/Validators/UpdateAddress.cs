using FluentValidation;
using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Validators
{
    public class UpdateAddress : AbstractValidator<Commands.UpdateAddress>
    {
        public UpdateAddress()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.StreetNumber).NotEmpty();
            RuleFor(x => x.StreetNumberSufix).Length(0, 5);
            RuleFor(x => x.StreetName).NotEmpty();
            RuleFor(x => x.StreetDirection).Length(0, 2);
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.District).NotEmpty();
            RuleFor(x => x.PostalArea).NotEmpty();
            RuleFor(x => x.CountryId).NotEmpty();
        }
    }
}