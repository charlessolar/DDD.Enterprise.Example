using Demo.Domain.Configuration.AccountType;
using Demo.Library.Extensions;
using NServiceBus;
using Seed.Attributes;
using Seed.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands = Demo.Domain.Configuration.AccountType.Commands;
using Type = Seed.Types.Configuration;

namespace Seed.Operations
{
    [Operation("Account Type")]
    [Category("Configuration")]
    public class AccountType : IOperation
    {
        public static IEnumerable<Type.AccountType> Data = new[] {
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Assets", DeferralMethod = DEFERRAL_METHOD.Unreconciled },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Liabilities", DeferralMethod = DEFERRAL_METHOD.Unreconciled },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Income", DeferralMethod = DEFERRAL_METHOD.None },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Expenses", DeferralMethod = DEFERRAL_METHOD.None },
        };

        public static IEnumerable<Type.AccountType> Assets = new[] {
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Bank", DeferralMethod = DEFERRAL_METHOD.Balance, Parent = Data.ElementAt(0) },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Current Asset", DeferralMethod = DEFERRAL_METHOD.Balance, Parent = Data.ElementAt(0) },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Fixed Asset", DeferralMethod = DEFERRAL_METHOD.Balance, Parent = Data.ElementAt(0) },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Accumulated Depreciation", DeferralMethod = DEFERRAL_METHOD.Balance, Parent = Data.ElementAt(0) },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Receivable", DeferralMethod = DEFERRAL_METHOD.Unreconciled, Parent = Data.ElementAt(0) },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Warehouse", DeferralMethod = DEFERRAL_METHOD.Balance, Parent = Data.ElementAt(0) },
        };

        public static IEnumerable<Type.AccountType> Liabilities = new[] {
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Equity", DeferralMethod = DEFERRAL_METHOD.Balance, Parent = Data.ElementAt(1) },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Payable", DeferralMethod = DEFERRAL_METHOD.Unreconciled, Parent = Data.ElementAt(1) },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Current Liability", DeferralMethod = DEFERRAL_METHOD.Balance, Parent = Data.ElementAt(1) },
            new Type.AccountType { Id = Guid.NewGuid(), Name = "Long-Term Liability", DeferralMethod = DEFERRAL_METHOD.Balance, Parent = Data.ElementAt(1) },
        };

        private readonly IBus _bus;

        public AccountType(IBus bus)
        {
            _bus = bus;
        }

        public async Task<Boolean> Seed()
        {
            var commands = Data.Select(x => new Commands.Create { AccountTypeId = x.Id, Name = x.Name, DeferralMethod = x.DeferralMethod, ParentId = x.Parent == null ? (Guid?)null : x.Parent.Id, Timestamp = DateTime.UtcNow, UserId = "IMPORT" });
            await commands.WhenAllAsync(x => _bus.Send(x).IsCommand<Command>());

            var assets = Assets.Select(x => new Commands.Create { AccountTypeId = x.Id, Name = x.Name, DeferralMethod = x.DeferralMethod, ParentId = x.Parent == null ? (Guid?)null : x.Parent.Id, Timestamp = DateTime.UtcNow, UserId = "IMPORT" });
            await assets.WhenAllAsync(x => _bus.Send(x).IsCommand<Command>());

            var liabilities = Liabilities.Select(x => new Commands.Create { AccountTypeId = x.Id, Name = x.Name, DeferralMethod = x.DeferralMethod, ParentId = x.Parent == null ? (Guid?)null : x.Parent.Id, Timestamp = DateTime.UtcNow, UserId = "IMPORT" });
            await liabilities.WhenAllAsync(x => _bus.Send(x).IsCommand<Command>());

            this.Done = true;
            return this.Done;
        }

        public Boolean Done { get; private set; }
    }
}