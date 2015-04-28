using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }
        public String Identity { get; set; }

        public String FullName { get; set; }

        public DateTime? Hired { get; set; }

        public DateTime? Terminated { get; set; }

        public String Phone { get; set; }

        public String Gender { get; set; }
    }
}