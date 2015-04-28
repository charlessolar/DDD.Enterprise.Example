using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.AccountType.ValueObjects
{
    public class Description : Aggregates.ValueObject<Description>
    {
        public readonly String Value;

        public Description(String Description)
        {
            this.Value = Description;
        }
    }
}