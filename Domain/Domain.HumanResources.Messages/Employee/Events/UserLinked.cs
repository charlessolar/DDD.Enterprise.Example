using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee.Events
{
    public interface UserLinked : IEvent
    {
        Guid EmployeeId { get; set; }

        String UserId { get; set; }
    }
}