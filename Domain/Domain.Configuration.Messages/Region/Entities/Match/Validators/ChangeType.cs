using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Validators
{
    public class ChangeType : AbstractValidator<Commands.ChangeType>
    {
        public ChangeType()
        {
            RuleFor(x => x.RegionId).NotEmpty();
            RuleFor(x => x.MatchId).NotEmpty();
            RuleFor(x => x.Type).NotNull();
        }
    }
}