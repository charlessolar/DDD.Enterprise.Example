using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Validators
{
    public class UpdateEmail : AbstractValidator<Commands.UpdateEmail>
    {
        public UpdateEmail()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.Email).EmailAddress();
        }
    }
}