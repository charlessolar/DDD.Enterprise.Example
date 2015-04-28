using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Validators
{
    public class ChangeResponsible : AbstractValidator<Commands.ChangeResponsible>
    {
        public ChangeResponsible()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
