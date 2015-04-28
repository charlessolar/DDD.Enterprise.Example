using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.ValueObjects
{
    public class SkipDraft : Aggregates.ValueObject<SkipDraft>
    {
        public readonly Boolean Value;

        public SkipDraft(Boolean SkipDraft)
        {
            this.Value = SkipDraft;
        }
    }
}