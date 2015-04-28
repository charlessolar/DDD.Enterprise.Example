using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Authentication.Users
{
    public interface IUser : Aggregates.Contracts.IEventSource<String>
    {
    }
}