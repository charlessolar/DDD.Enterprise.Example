using Demo.Domain.Configuration.AccountType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Types.Configuration
{
    public class AccountType
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public DEFERRAL_METHOD DeferralMethod { get; set; }

        public AccountType Parent { get; set; }
    }
}