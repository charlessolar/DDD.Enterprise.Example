using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Commands
{
    public class UpdateNationalId : DemoCommand
    {
        public Guid EmployeeId { get; set; }

        public String NationalId { get; set; }
    }
}