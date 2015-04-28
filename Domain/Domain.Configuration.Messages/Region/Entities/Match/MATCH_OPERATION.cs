using Aggregates;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match
{
    public class MATCH_OPERATION : Enumeration<MATCH_OPERATION, String>
    {
        public static MATCH_OPERATION EQUAL = new MATCH_OPERATION("EQUAL", "Equal");
        public static MATCH_OPERATION NOT_EQUAL = new MATCH_OPERATION("NOT_EQUAL", "Not Equal");
        public static MATCH_OPERATION CONTAIN = new MATCH_OPERATION("CONTAIN", "Contains");
        public static MATCH_OPERATION NOT_CONTAIN = new MATCH_OPERATION("NOT_CONTAIN", "Not Contains");

        public MATCH_OPERATION(String value, String displayName)
            : base(value, displayName)
        { }
    }
}