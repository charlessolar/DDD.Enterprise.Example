using FluentValidation.Results;
using System;
using System.Collections.Generic;

namespace Demo.Library.Command
{
    public class Reject : Aggregates.Messages.Reject
    {
        public String Message { get; set; }

        public IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}