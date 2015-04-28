using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Authentication.Users.Responses
{
    public class Get : IResponse, IHasStringId
    {
        public String Id { get; set; }
        public Guid? EmployeeId { get; set; }

        public String Name { get; set; }

        public String Email { get; set; }

        public String NickName { get; set; }

        public String Timezone { get; set; }

        public String ImageType { get; set; }

        public String ImageData { get; set; }
    }
}