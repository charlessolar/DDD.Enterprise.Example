using Demo.Library.Responses;
using ServiceStack;
using System;

namespace Demo.Application.ServiceStack.Accounting.Account.Services
{
    [Api("Accounting")]
    [Route("/accounting/account", "POST", Summary = "Account.Create")]
    public class Create : IReturn<Base<Command>>
    {
        public Guid AccountId { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Boolean AcceptPayments { get; set; }

        public Boolean AllowReconcile { get; set; }

        public String Operation { get; set; }

        public Guid StoreId { get; set; }

        public Guid CurrencyId { get; set; }
    }
}