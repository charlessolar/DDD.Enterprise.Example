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
    public class UpdateDirectPhone : AbstractValidator<Services.UpdateDirectPhone>
    {
        public UpdateDirectPhone()
        {
            RuleFor(x => x.EmployeeId).NotEmpty();
        }
    }
}