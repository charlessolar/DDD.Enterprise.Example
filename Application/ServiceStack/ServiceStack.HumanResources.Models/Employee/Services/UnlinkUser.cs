using Demo.Library.Responses;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Services
{
    [Api("HumanResources")]
    [Route("/human-resources/employee/{EmployeeId}/unlink", "POST")]
    public class UnlinkUser : IReturn<Base<Command>>
    {
        public Guid EmployeeId { get; set; }
    }
}