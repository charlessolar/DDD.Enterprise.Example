using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Validators
{
    public class ChangeName : AbstractValidator<Commands.ChangeName>
    {
        public ChangeName()
        {
            RuleFor(x => x.RegionId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}