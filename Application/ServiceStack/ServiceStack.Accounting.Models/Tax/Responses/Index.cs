using Demo.Library.Responses;
using ServiceStack.Model;
using System;

namespace Demo.Application.ServiceStack.Accounting.Tax.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Boolean Fixed { get; set; }

        public Decimal Rate { get; set; }

        public Guid TypeId { get; set; }

        public String Type { get; set; }

        public String Account { get; set; }

        public Boolean Activated { get; set; }
    }
}