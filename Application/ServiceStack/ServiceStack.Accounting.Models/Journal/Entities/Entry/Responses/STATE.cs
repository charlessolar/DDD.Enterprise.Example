using Aggregates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Entry.Responses
{
    public class STATE : Enumeration<STATE, String>
    {
        public static STATE New = new STATE("NEW", "New", "Draft journal entry");
        public static STATE Aborted = new STATE("ABORTED", "Aborted", "Entry aborted, not posted");
        public static STATE Exception = new STATE("EXCEPTION", "Exception", "Entry under exception, see details");
        public static STATE UnderReview = new STATE("UNDERREVIEW", "Under Review", "Needs review");
        public static STATE Closed = new STATE("CLOSED", "Closed", "Closed, complete");

        public STATE(String value, String displayName, String description)
            : base(value, displayName)
        {
            this.Description = description;
        }

        public string Description { get; private set; }
    }
}