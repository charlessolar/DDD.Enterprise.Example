using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType
{
    public class DEFERRAL_METHOD : Enumeration<DEFERRAL_METHOD, String>
    {
        public static DEFERRAL_METHOD None = new DEFERRAL_METHOD("NONE", "None", "Nothing will be done");
        public static DEFERRAL_METHOD Balance = new DEFERRAL_METHOD("BALANCE", "Balance", "Will generally be used for cash accounts");
        public static DEFERRAL_METHOD Detail = new DEFERRAL_METHOD("DETAIL", "Detail", "Will copy every entry to the next journal, even the reconciled ones");
        public static DEFERRAL_METHOD Unreconciled = new DEFERRAL_METHOD("UNRECONCILED", "Unreconciled", "Will copy only the entries that were unreconciled on the first day of the new fiscal year");

        public DEFERRAL_METHOD(String value, String displayName, String description)
            : base(value, displayName)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}