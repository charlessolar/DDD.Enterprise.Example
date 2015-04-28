using Demo.Domain.Configuration.AccountType;
using Demo.Library.Responses;
using ServiceStack.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Configuration.AccountType.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String DeferralMethod { get; set; }

        public Guid? ParentId { get; set; }
    }
}