using Demo.Domain.Accounting.Account;
using Demo.Library.Responses;
using ServiceStack.Model;
using System;

namespace Demo.Application.ServiceStack.Accounting.Account.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Guid? TypeId { get; set; }

        public String Type { get; set; }

        public String Operation { get; set; }

        public String Parent { get; set; }

        public String ParentCode { get; set; }

        public Guid? ParentId { get; set; }

        public Boolean Frozen { get; set; }

        public Guid StoreId { get; set; }

        public String Store { get; set; }

        public Guid CurrencyId { get; set; }

        public String Currency { get; set; }
    }
}