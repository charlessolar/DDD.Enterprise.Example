using Demo.Library.Command;
using Demo.Library.Responses;
using ServiceStack;
using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Validators
{
    public class UpdateGender : AbstractValidator<Services.UpdateGender>
    {
        public UpdateGender()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
            RuleFor(x => x.Gender).NotEmpty();
        }
    }
}