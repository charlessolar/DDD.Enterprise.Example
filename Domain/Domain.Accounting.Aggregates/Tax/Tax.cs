using Aggregates.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax
{
    public class Tax : Aggregates.Aggregate<Guid>, ITax
    {
        public Aggregates.SingleValueObject<Boolean> Activated { get; private set; }

        public ValueObjects.Rate Rate { get; private set; }

        private Tax()
        {
            this.Activated = new Aggregates.SingleValueObject<bool>(false);
        }

        public void Create(String Code, String Name, String Description, Configuration.TaxType.ITaxType TaxType)
        {
            Apply<Events.Created>(e =>
            {
                e.TaxId = Id;
                e.Code = Code;
                e.Name = Name;
                e.Description = Description;
                e.TaxTypeId = TaxType.Id;
            });
        }

        public void Activate(Authentication.Users.IUser user)
        {
            if (this.Rate == null)
                throw new BusinessException("Activate without rate");

            Apply<Events.Activated>(e =>
            {
                e.TaxId = Id;
                e.UserId = user.Id;
            });
        }

        private void Handle(Events.Activated e)
        {
            this.Activated = new Aggregates.SingleValueObject<bool>(true);
        }

        public void AddRegion(Configuration.Region.IRegion Region)
        {
            Apply<Events.RegionAdded>(e =>
            {
                e.TaxId = Id;
                e.RegionId = Region.Id;
            });
        }

        public void AddStore(Relations.Store.IStore Store)
        {
            Apply<Events.StoreAdded>(e =>
            {
                e.TaxId = Id;
                e.StoreId = Store.Id;
            });
        }

        public void ChangeAccount(Accounting.Account.IAccount Account)
        {
            Apply<Events.AccountChanged>(e =>
            {
                e.TaxId = Id;
                e.AccountId = Account.Id;
            });
        }

        public void ChangeDescription(String Description)
        {
            Apply<Events.DescriptionChanged>(e =>
            {
                e.TaxId = Id;
                e.Description = Description;
            });
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameChanged>(e =>
            {
                e.TaxId = Id;
                e.Name = Name;
            });
        }

        public void ChangeRate(Boolean Fixed, Decimal Rate)
        {
            Apply<Events.RateChanged>(e =>
            {
                e.TaxId = Id;
                e.Fixed = Fixed;
                e.Rate = Rate;
            });
        }

        private void Handle(Events.RateChanged e)
        {
            this.Rate = new ValueObjects.Rate(e.Fixed, e.Rate);
        }

        public void Deactivate(Authentication.Users.IUser user)
        {
            Apply<Events.Deactivated>(e =>
            {
                e.TaxId = Id;
                e.UserId = user.Id;
            });
        }

        private void Handle(Events.Deactivated e)
        {
            this.Activated = new Aggregates.SingleValueObject<bool>(false);
        }

        public void Destroy()
        {
            if (this.Activated.Value)
                throw new BusinessException("Tax is active");

            Apply<Events.Destroyed>(e =>
            {
                e.TaxId = Id;
            });
        }

        public void RemoveRegion(Configuration.Region.IRegion Region)
        {
            Apply<Events.RegionRemoved>(e =>
            {
                e.TaxId = Id;
                e.RegionId = Region.Id;
            });
        }

        public void RemoveStore(Relations.Store.IStore Store)
        {
            Apply<Events.StoreRemoved>(e =>
            {
                e.TaxId = Id;
                e.StoreId = Store.Id;
            });
        }
    }
}