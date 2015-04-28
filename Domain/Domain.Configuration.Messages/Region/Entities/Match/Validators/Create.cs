using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Entities.Match.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.RegionId).NotEmpty();
            RuleFor(x => x.MatchId).NotEmpty();
            RuleFor(x => x.Value).NotEmpty();
            RuleFor(x => x.Type).NotNull();
            RuleFor(x => x.Operation).NotNull();
        }
    }
}