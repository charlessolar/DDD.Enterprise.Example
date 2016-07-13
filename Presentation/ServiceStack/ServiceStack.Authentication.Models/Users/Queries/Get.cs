using Demo.Library.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Authentication.Users.Queries
{
    public interface Get : IQuery
    {
        String UserAuthId { get; set; }
    }
}
