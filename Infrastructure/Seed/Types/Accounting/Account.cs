using Demo.Domain.Accounting.Account;
using Seed.Types.Configuration;
using Seed.Types.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Types.Accounting
{
    public class Account
    {
        public Guid Id { get; set; }

        public String Code { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Boolean AcceptPayments { get; set; }

        public Boolean AllowReconcile { get; set; }

        public AccountType Type { get; set; }

        public OPERATION Operation { get; set; }


        public Currency Currency { get; set; }

        public Boolean Activated { get; set; }

        public Decimal Balance { get; set; }

        public ICollection<Account> Children { get; set; }
    }
}