using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Messages;
using FluentValidation.Results;

namespace Demo.Library.Command
{
    public interface RejectValidation : Reject
    {
        IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}