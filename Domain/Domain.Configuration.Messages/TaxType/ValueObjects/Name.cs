using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.TaxType.ValueObjects
{
    public class Name : Aggregates.ValueObject<Name>
    {
        public readonly String Value;

        public Name(String Name)
        {
            this.Value = Name;
        }
    }
}