using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Relations.Store.Responses
{
    public class Get : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Identity { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Address Address { get; set; }

        public Guid CurrencyId { get; set; }

        public String Currency { get; set; }

        public String Phone { get; set; }

        public String Fax { get; set; }

        public String Email { get; set; }

        public String Website { get; set; }
    }
}