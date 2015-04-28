using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.HumanResources.Employee.Responses
{
    public class Get : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }
        public String Identity { get; set; }

        public String FullName { get; set; }

        public DateTime? Hired { get; set; }

        public DateTime? Terminated { get; set; }

        public String UserId { get; set; }

        public Address Address { get; set; }
        public Guid CurrencyId { get; set; }
        public String Currency { get; set; }

        public String Phone { get; set; }
        public String DirectPhone { get; set; }

        public String Fax { get; set; }

        public String Mobile { get; set; }
        public String Email { get; set; }
        public String Gender { get; set; }

        public String MaritalStatus { get; set; }

        public String NationalId { get; set; }

        public String Website { get; set; }
    }
}