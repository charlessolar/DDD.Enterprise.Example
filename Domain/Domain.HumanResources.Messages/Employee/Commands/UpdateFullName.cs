using Demo.Library.Command;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Commands
{
    public class UpdateFullName : DemoCommand
    {
        public Guid EmployeeId { get; set; }

        public String FullName { get; set; }
    }
}