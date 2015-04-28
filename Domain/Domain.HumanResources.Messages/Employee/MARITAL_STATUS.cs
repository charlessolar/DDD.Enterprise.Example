using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.HumanResources.Employee
{
    public class MARITAL_STATUS : Aggregates.Enumeration<MARITAL_STATUS, String>
    {
        public MARITAL_STATUS SINGLE = new MARITAL_STATUS("SINGLE", "Single");
        public MARITAL_STATUS MARRIED = new MARITAL_STATUS("MARRIED", "Married");
        public MARITAL_STATUS DIVORCED = new MARITAL_STATUS("DIVORCED", "Divorced");
        public MARITAL_STATUS WIDOWED = new MARITAL_STATUS("WIDOWED", "Widowed");
        public MARITAL_STATUS COHABITATING = new MARITAL_STATUS("COHABITATING", "Cohabitating");
        public MARITAL_STATUS CIVIL_UNION = new MARITAL_STATUS("CIVIL_UNION", "Civil Union");
        public MARITAL_STATUS DOMESTIC_PARTNERSHIP = new MARITAL_STATUS("DOMESTIC_PARTNERSHIP", "Domestic Partnership");
        public MARITAL_STATUS UNMARRIED_PARTNERS = new MARITAL_STATUS("UNMARRIED_PARTNERS", "Unmarried Partners");
        public MARITAL_STATUS OTHER = new MARITAL_STATUS("OTHER", "Other");

        public MARITAL_STATUS(String value, String displayName) :
            base(value, displayName) { }
    }
}