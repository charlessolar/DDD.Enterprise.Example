using Demo.Library.Responses;
using ServiceStack.Model;
using System;

namespace Demo.Application.ServiceStack.Accounting.Journal.Responses
{
    public class Index : IResponse, IHasGuidId
    {
        public Guid Id { get; set; }

        public Boolean Closed { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public Guid ResponsibleId { get; set; }

        public String Responsible { get; set; }

        public String DebitAccount { get; set; }

        public String CreditAccount { get; set; }
    }
}