using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType
{
    public class AccountType : Aggregates.Aggregate<Guid>, IAccountType
    {
        public ValueObjects.Name Name { get; private set; }

        public ValueObjects.Description Description { get; private set; }

        public DEFERRAL_METHOD DeferralMethod { get; private set; }

        private AccountType()
        {
        }

        public void ChangeDeferral(DEFERRAL_METHOD deferral)
        {
            Apply<Events.DeferralChanged>(e =>
            {
                e.AccountTypeId = Id;
                e.DeferralMethod = deferral.Value;
            });
        }

        private void Handle(Events.DeferralChanged e)
        {
            this.DeferralMethod = DEFERRAL_METHOD.FromValue(e.DeferralMethod);
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
            this.Description = new ValueObjects.Description(e.Description);
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
            this.Name = new ValueObjects.Name(e.Name);
        }

        public void Create(String Name, DEFERRAL_METHOD Deferral, Guid? ParentId)
        {
            Apply<Events.Created>(e =>
            {
                e.AccountTypeId = Id;
                e.Name = Name;
                e.DeferralMethod = Deferral.Value;
                e.ParentId = ParentId;
            });
        }

        private void Handle(Events.Created e)
        {
            this.Name = new ValueObjects.Name(e.Name);
            this.DeferralMethod = DEFERRAL_METHOD.FromValue(e.DeferralMethod);
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.AccountTypeId = Id;
            });
        }
    }
}