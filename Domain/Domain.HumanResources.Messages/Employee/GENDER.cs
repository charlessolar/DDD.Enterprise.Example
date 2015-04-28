using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee
{
    public class GENDER : Aggregates.Enumeration<GENDER, String>
    {
        public GENDER MALE = new GENDER("MALE", "Male");
        public GENDER FEMALE = new GENDER("FEMALE", "Female");
        public GENDER OTHER = new GENDER("OTHER", "Other");

        public GENDER(String value, String displayName) :
            base(value, displayName) { }
    }
}