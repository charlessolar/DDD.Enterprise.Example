using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Validators
{
    public class LinkUser : AbstractValidator<Commands.LinkUser>
    {
        public LinkUser()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}