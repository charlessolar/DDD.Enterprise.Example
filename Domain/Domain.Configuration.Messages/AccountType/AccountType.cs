using Aggregates.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType
{
    public class AccountType : Aggregates.Aggregate<Guid>
    {
        public Boolean Selectable { get; private set; }

        public String Name { get; private set; }

        public String Description { get; private set; }

        public DEFERRAL_METHOD DeferralMethod { get; private set; }

        private AccountType()
        {
        }

        public void ChangeDeferral(DEFERRAL_METHOD deferral)
        {
            Apply<Events.DeferralChanged>(e =>
            {
                e.AccountTypeId = Id;
                e.DeferralMethod = deferral;
            });
        }

        private void Handle(Events.DeferralChanged e)
        {
            this.DeferralMethod = e.DeferralMethod;
        }

        public void ChangeDescription(String Description)
        {
            Apply<Events.DescriptionChanged>(e =>
            {
                e.AccountTypeId = Id;
                e.Description = Description;
            });
        }

        private void Handle(Events.DescriptionChanged e)
        {
            this.Description = e.Description;
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameChanged>(e =>
            {
                e.AccountTypeId = Id;
                e.Name = Name;
            });
        }

        private void Handle(Events.NameChanged e)
        {
            this.Name = e.Name;
        }

        public void Create(String Name, Boolean Selectable, DEFERRAL_METHOD Deferral, Guid? ParentId)
        {
            Apply<Events.Created>(e =>
            {
                e.AccountTypeId = Id;
                e.Name = Name;
                e.Selectable = Selectable;
                e.DeferralMethod = Deferral;
                e.ParentId = ParentId;
            });
        }

        private void Handle(Events.Created e)
        {
            this.Name = e.Name;
            this.Selectable = e.Selectable;
            this.DeferralMethod = e.DeferralMethod;
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.AccountTypeId = Id;
            });
        }

        public void ChangeSelectable(Boolean Selectable)
        {
            Apply<Events.Selectable>(e =>
            {
                e.AccountTypeId = Id;
                e.Selectable = Selectable;
            });
        }

        private void Handle(Events.Selectable e)
        {
            this.Selectable = e.Selectable;
        }
    }
}