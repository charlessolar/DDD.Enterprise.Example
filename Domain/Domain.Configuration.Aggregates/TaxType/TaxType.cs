using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.TaxType
{
    public class TaxType : Aggregates.Aggregate<Guid>, ITaxType
    {
        public ValueObjects.Name Name { get; private set; }

        private TaxType()
        {
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameChanged>(e =>
            {
                e.TaxTypeId = Id;
                e.Name = Name;
            });
        }

        private void Handle(Events.NameChanged e)
        {
            this.Name = new ValueObjects.Name(e.Name);
        }

        public void Create(String Name)
        {
            Apply<Events.Created>(e =>
            {
                e.TaxTypeId = Id;
                e.Name = Name;
            });
        }

        private void Handle(Events.Created e)
        {
            this.Name = new ValueObjects.Name(e.Name);
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.TaxTypeId = Id;
            });
        }
    }
}