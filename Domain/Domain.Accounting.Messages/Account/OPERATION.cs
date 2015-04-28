using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account
{
    public class OPERATION : Enumeration<OPERATION, String>
    {
        public static OPERATION Group = new OPERATION("GROUP", "Group", "Can have child accounts, can not have journal entries");
        public static OPERATION Ledger = new OPERATION("LEDGER", "Ledger", "A regular account");
        public static OPERATION Consolidation = new OPERATION("CONSOLIDATION", "Consolidation", "For multi-company corporations, can have child accounts");

        public OPERATION(String value, String displayName, String description)
            : base(value, displayName)
        {
            this.Description = description;
        }

        public String Description { get; private set; }
    }
}