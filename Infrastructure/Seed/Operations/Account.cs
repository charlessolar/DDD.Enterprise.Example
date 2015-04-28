using Demo.Domain.Accounting.Account;
using Demo.Library.Extensions;
using NServiceBus;
using Seed.Attributes;
using Seed.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands = Demo.Domain.Accounting.Account.Commands;
using Type = Seed.Types.Accounting;

namespace Seed.Operations
{
    [Operation("Account")]
    [Depends("User", "Account Type", "Currency")]
    [Category("Accounting")]
    public class Account : IOperation
    {
        public static IEnumerable<Type.Account> Data = new[] {
            new Type.Account { Id = Guid.NewGuid(), Code = "0", Name = "Demo", Description = "Company account", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Children = new[] {
                new Type.Account { Id = Guid.NewGuid(), Code = "1", Name = "Assets", Description = "Application of funds", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(0), Children = new[] {
                    new Type.Account { Id = Guid.NewGuid(), Code = "0", Name = "Current Assets", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Assets.ElementAt(1), Children = new[] {
                        new Type.Account { Id = Guid.NewGuid(), Code = "0", Name = "Receivable", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(4), Children = new[] {
                            new Type.Account { Id = Guid.NewGuid(), Code = "10", Name = "Account Receivable", Operation = OPERATION.Ledger, Activated = true, AllowReconcile = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Assets.ElementAt(4) }
                        }},
                        new Type.Account { Id = Guid.NewGuid(), Code = "1", Name = "Bank Accounts", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(0), Children = new[] {
                            new Type.Account { Id = Guid.NewGuid(), Code = "00", Name = "Bank", Operation = OPERATION.Ledger, Activated = true, AcceptPayments = true, AllowReconcile = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(0) }
                        }},
                        new Type.Account { Id = Guid.NewGuid(), Code = "2", Name = "Cash in Hand", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Assets.ElementAt(0), Children = new[] {
                            new Type.Account { Id = Guid.NewGuid(), Code = "00", Name = "Petty Cash", Operation = OPERATION.Ledger, Activated = true, AcceptPayments = true, AllowReconcile = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(0) }
                        }},
                        new Type.Account { Id = Guid.NewGuid(), Code = "3", Name = "Loans and Advances", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(1) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "4", Name = "Securities and Deposits", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(1) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "5", Name = "Inventory", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(1), Children = new[] {
                            new Type.Account { Id = Guid.NewGuid(), Code = "00", Name = "Inventory", Operation = OPERATION.Ledger, Activated = true, AllowReconcile = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Assets.ElementAt(5) },
                            new Type.Account { Id = Guid.NewGuid(), Code = "01", Name = "Finished Goods", Operation = OPERATION.Ledger, Activated = true, AllowReconcile = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(5) },
                            new Type.Account { Id = Guid.NewGuid(), Code = "02", Name = "Work in Progress", Operation = OPERATION.Ledger, Activated = true, AllowReconcile = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Assets.ElementAt(5) }
                        }},
                        new Type.Account { Id = Guid.NewGuid(), Code = "6", Name = "Taxes", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Assets.ElementAt(1) }
                    }}
                }},

                new Type.Account { Id = Guid.NewGuid(), Code = "2", Name = "Liabilities and Equity", Description = "Source of funds", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Children = new[] {
                    new Type.Account { Id = Guid.NewGuid(), Code = "0", Name = "Liabilities", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(1), Children = new[] {
                        new Type.Account { Id = Guid.NewGuid(), Code = "0", Name = "Current Liabilities", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Liabilities.ElementAt(2), Children = new[] {
                            new Type.Account { Id = Guid.NewGuid(), Code = "0", Name = "Payable", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Liabilities.ElementAt(1), Children = new[] {
                                new Type.Account { Id = Guid.NewGuid(), Code = "10", Name = "Account Payable", Operation = OPERATION.Ledger, Activated = true, AllowReconcile = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Liabilities.ElementAt(1) },
                            }},
                            new Type.Account { Id = Guid.NewGuid(), Code = "4", Name = "Duties and Taxes", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Liabilities.ElementAt(2) },
                            new Type.Account { Id = Guid.NewGuid(), Code = "5", Name = "Loans", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Liabilities.ElementAt(2) },
                            new Type.Account { Id = Guid.NewGuid(), Code = "6", Name = "Stocks", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Liabilities.ElementAt(2) },
                        }},
                        new Type.Account { Id = Guid.NewGuid(), Code = "3", Name = "Equity", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Liabilities.ElementAt(0), Children = new[] {
                            new Type.Account { Id = Guid.NewGuid(), Code = "9", Name = "Opening Balance", Description = "Offset for opening balance transactions", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),Type = AccountType.Liabilities.ElementAt(0) },
                            new Type.Account { Id = Guid.NewGuid(), Code = "1", Name = "Dividends Paid", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Liabilities.ElementAt(0) },
                            new Type.Account { Id = Guid.NewGuid(), Code = "2", Name = "Retained Earnings", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Liabilities.ElementAt(0) },
                        }},
                    }},
                }},
                new Type.Account { Id = Guid.NewGuid(), Code = "4", Name = "Income", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(2), Children = new[] {
                    new Type.Account { Id = Guid.NewGuid(), Code = "0", Name = "Direct Income", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(2), Children = new[] {
                        new Type.Account { Id = Guid.NewGuid(), Code = "500", Name = "Sales", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(2) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "700", Name = "Sales Discounts", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(2) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "800", Name = "Shipping and Delivery", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(2) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "900", Name = "Miscellaneous", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(2) },
                    }},
                    new Type.Account { Id = Guid.NewGuid(), Code = "1", Name = "Indirect Income", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(2) },
                }},
                new Type.Account { Id = Guid.NewGuid(), Code = "6", Name = "Expenses", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(3), Children = new []{
                    new Type.Account { Id = Guid.NewGuid(), Code = "0", Name = "Direct Expenses", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3), Children = new[] {
                        new Type.Account { Id = Guid.NewGuid(), Code = "010", Name = "Cost of Goods Sold", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "900", Name = "Inventory Adjustment", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                    }},
                    new Type.Account { Id = Guid.NewGuid(), Code = "1", Name = "Indirect Expenses", Operation = OPERATION.Group, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3), Children = new[] {
                        new Type.Account { Id = Guid.NewGuid(), Code = "010", Name = "Advertising", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "020", Name = "Automobile", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "030", Name = "Bank Charges", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "150", Name = "Licenses and Permits", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "160", Name = "Computers and Internet", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "170", Name = "Charitable Contributions", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "200", Name = "Depreciation", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "300", Name = "Equipment Rental", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "410", Name = "Meals and Entertainment", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "420", Name = "Office Supplies", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "600", Name = "Payroll", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "610", Name = "Professional Fees", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "700", Name = "Rent", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "710", Name = "Repairs and Maintenance", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "800", Name = "Taxes - Property", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "810", Name = "Travel", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "820", Name = "Utilities", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0),  Type = AccountType.Data.ElementAt(3) },
                        new Type.Account { Id = Guid.NewGuid(), Code = "900", Name = "Miscellaneous", Operation = OPERATION.Ledger, Activated = true, Currency = Currency.Data.ElementAt(0), Type = AccountType.Data.ElementAt(3) },
                    }},
                }},
            }},
        };

        private readonly IBus _bus;

        public Account(IBus bus)
        {
            _bus = bus;
        }

        public async Task<Boolean> Seed()
        {
            var commands = MapCommands(Data);

            foreach (var account in commands)
                await _bus.Send(account.Create).IsCommand<Command>();

            foreach (var account in commands.Where(x => x.Type != null))
                await _bus.Send(account.Type).IsCommand<Command>();

            foreach (var account in commands.Where(x => x.Parent != null))
                await _bus.Send(account.Parent).IsCommand<Command>();

            this.Done = true;
            return this.Done;
        }

        private class Container
        {
            public Commands.Create Create { get; set; }

            public Commands.ChangeType Type { get; set; }

            public Commands.ChangeParent Parent { get; set; }
        }

        private IList<Container> MapCommands(IEnumerable<Type.Account> Accounts, Type.Account Parent = null)
        {
            return Accounts.SelectMany(x =>
            {
                var results = new List<Container>();
                results.Add(
                    new Container
                    {
                        Create = new Commands.Create
                        {
                            AccountId = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Operation = x.Operation,
                            AcceptPayments = x.AcceptPayments,
                            AllowReconcile = x.AllowReconcile,
                            CurrencyId = x.Currency.Id,
                            Timestamp = DateTime.UtcNow,
                            UserId = User.Data.ElementAt(0).Id
                        },
                        Type = x.Type == null ? null : new Commands.ChangeType
                        {
                            AccountId = x.Id,
                            TypeId = x.Type.Id,
                            Timestamp = DateTime.UtcNow,
                            UserId = User.Data.ElementAt(0).Id
                        },
                        Parent = Parent == null ? null : new Commands.ChangeParent
                        {
                            AccountId = x.Id,
                            ParentId = Parent.Id,
                            Timestamp = DateTime.UtcNow,
                            UserId = User.Data.ElementAt(0).Id
                        },
                    });
                if (x.Children != null && x.Children.Count > 0)
                    results.AddRange(MapCommands(x.Children, x));
                return results;
            }).ToList();
        }

        public Boolean Done { get; private set; }
    }
}