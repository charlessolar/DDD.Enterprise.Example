using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Validators
{
    public class ChangeReference : AbstractValidator<Commands.ChangeReference>
    {
        public ChangeReference()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Reference).NotEmpty();
        }
    }
}