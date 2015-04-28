using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType
{
    public interface IAccountType : Aggregates.Contracts.IEventSource<Guid>
    {
        ValueObjects.Name Name { get; }

        ValueObjects.Description Description { get; }

        DEFERRAL_METHOD DeferralMethod { get; }
    }
}