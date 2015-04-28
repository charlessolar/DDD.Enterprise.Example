using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Validators
{
    public class ChangeDescription : AbstractValidator<Commands.ChangeDescription>
    {
        public ChangeDescription()
        {
            RuleFor(x => x.RegionId).NotEmpty();
        }
    }
}