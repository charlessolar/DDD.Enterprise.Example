using Demo.Library.Responses;
using ServiceStack.Model;
using System;

namespace Demo.Application.ServiceStack.Accounting.Currency.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public String Symbol { get; set; }

        public Boolean Activated { get; set; }

        public String Format { get; set; }
    }
}