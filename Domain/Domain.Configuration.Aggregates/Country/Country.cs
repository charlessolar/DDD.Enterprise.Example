using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Country
{
    public class Country : Aggregates.Aggregate<Country, Guid>, ICountry
    {
        private Country()
        {
        }

        public void Create(String Code, String Name)
        {
            Apply<Events.Created>(e =>
            {
                e.CountryId = Id;
                e.Code = Code;
                e.Name = Name;
            });
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameUpdated>(e =>
            {
                e.CountryId = Id;
                e.Name = Name;
            });
        }

        public void Destroy()
        {
            Apply<Events.Destroyed>(e =>
            {
                e.CountryId = Id;
            });
        }
    }
}