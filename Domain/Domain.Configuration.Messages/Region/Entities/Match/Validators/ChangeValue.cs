using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Validators
{
    public class ChangeValue : AbstractValidator<Commands.ChangeValue>
    {
        public ChangeValue()
        {
            RuleFor(x => x.RegionId).NotEmpty();
            RuleFor(x => x.MatchId).NotEmpty();
            RuleFor(x => x.Value).NotEmpty();
        }
    }
}