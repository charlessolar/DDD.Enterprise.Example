using System.Collections.Generic;
using Aggregates.Messages;
using FluentValidation.Results;

namespace Demo.Domain.Infrastructure.Command
{
    public interface RejectValidation : Reject
    {
        IEnumerable<ValidationResult> ValidationResults { get; set; }
    }
}