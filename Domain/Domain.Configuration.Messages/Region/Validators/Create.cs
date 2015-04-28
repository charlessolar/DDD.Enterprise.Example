using FluentValidation;
using Demo.Library.Command;
using System;

namespace Demo.Domain.Configuration.Region.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.RegionId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(2, 8);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ParentId).NotEqual(Guid.Empty);
        }
    }
}