using Aggregates.Exceptions;
using Demo.Library.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Account
{
    public class Account : Aggregates.Aggregate<Guid>, IAccount
    {
        public Aggregates.SingleValueObject<Boolean> Frozen { get; private set; }

        private Account()
        {
            this.Frozen = new Aggregates.SingleValueObject<bool>(false);
        }

        public void Create(String Code, String Name, Boolean AcceptPayments, Boolean AllowReconcile, OPERATION Operation, Currency.ICurrency Currency)
        {
            Apply<Events.Created>(e =>
            {
                e.AccountId = Id;
                e.Code = Code;
                e.Name = Name;
                e.AcceptPayments = AcceptPayments;
                e.AllowReconcile = AllowReconcile;
                e.Operation = Operation;
                e.CurrencyId = Currency.Id;
            });
        }

        public void ChangeDescription(String Description)
        {
            Apply<Events.DescriptionChanged>(e =>
            {
                e.AccountId = Id;
                e.Description = Description;
            });
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameChanged>(e =>
            {
                e.AccountId = Id;
                e.Name = Name;
            });
        }

        public void ChangeType(Configuration.AccountType.IAccountType AccountType)
        {
            Apply<Events.TypeChanged>(e =>
            {
                e.AccountId = Id;
                e.TypeId = AccountType.Id;
            });
        }

        public void ChangeParent(IAccount Parent)
        {
            Apply<Events.ParentChanged>(e =>
            {
                e.AccountId = Id;
                e.ParentId = Parent.Id;
            });
        }

        public void Freeze()
        {
            Apply<Events.Frozen>(e =>
            {
                e.AccountId = Id;
            });
        }

        public void Unfreeze()
        {
            Apply<Events.Unfrozen>(e =>
            {
                e.AccountId = Id;
            });
        }

        private void Handle(Events.Frozen e)
        {
            this.Frozen = new Aggregates.SingleValueObject<bool>(true);
        }

        private void Handle(Events.Unfrozen e)
        {
            this.Frozen = new Aggregates.SingleValueObject<bool>(false);
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.AccountId = Id;
            });
        }
    }
}