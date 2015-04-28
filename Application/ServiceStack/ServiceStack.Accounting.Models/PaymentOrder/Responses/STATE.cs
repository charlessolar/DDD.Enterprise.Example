using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.PaymentOrder.Responses
{
    public class STATE : Enumeration<STATE, String>
    {
        public static STATE New = new STATE("NEW", "New", "Draft Payment Order");
        public static STATE Discarded = new STATE("DISCARDED", "Discarded", "Discarded and unsable");
        public static STATE Confirmed = new STATE("CONFIRMED", "Confirmed", "Payment order confirmed, ready for payment");
        public static STATE Dispursed = new STATE("DISPURSED", "Dispursed", "Payments dispursed");

        public STATE(String value, String displayName, String description)
            : base(value, displayName)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}