using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seed.Types.Relations
{
    public class Store
    {
        public Guid Id { get; set; }

        public String Identity { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Address Address { get; set; }

        public Types.Accounting.Currency Currency { get; set; }

        public String Phone { get; set; }

        public String Fax { get; set; }

        public String Email { get; set; }

        public String Website { get; set; }
    }
}