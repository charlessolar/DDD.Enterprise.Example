using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Configuration.Region.Entities.Match
{
    public class MATCH_TYPE : Enumeration<MATCH_TYPE, String>
    {
        public static MATCH_TYPE STREET_NUMBER = new MATCH_TYPE("STREET_NUMBER", "Street Number", "The street address number");
        public static MATCH_TYPE STREET_NAME = new MATCH_TYPE("STREET_NAME", "Street Name", "The street name");
        public static MATCH_TYPE STREET_TYPE = new MATCH_TYPE("STREET_TYPE", "Street Type", "Type of street address (Dr, St, Ln, Cir)");
        public static MATCH_TYPE ADDRESS_TYPE = new MATCH_TYPE("ADDRESS_TYPE", "Address Type", "Type of the sub address (Suite, Apt, Unit)");
        public static MATCH_TYPE ADDRESS_TYPE_ID = new MATCH_TYPE("ADDRESS_TYPE_ID", "Address Type Identifier", "The sub address identifier");
        public static MATCH_TYPE MINOR_MUNI = new MATCH_TYPE("MINOR_MUNI", "Minor Municipality", "The minor municipality (Hamlet, Village)");
        public static MATCH_TYPE CITY = new MATCH_TYPE("CITY", "City", "The city/town");
        public static MATCH_TYPE DISTRICT = new MATCH_TYPE("DISTRICT", "District", "The governing district (State, Province, County)");
        public static MATCH_TYPE POSTAL_AREA = new MATCH_TYPE("POSTAL_AREA", "Postal Area", "The postcode (Zip, Postal Code, Postcode)");
        public static MATCH_TYPE COUNTRY = new MATCH_TYPE("COUNTRY", "Country", "The county");

        public MATCH_TYPE(String value, String displayName, String description)
            : base(value, displayName)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}
