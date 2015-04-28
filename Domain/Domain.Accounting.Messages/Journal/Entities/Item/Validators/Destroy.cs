using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Validators
{
    public class Destroy : AbstractValidator<Commands.Destroy>
    {
        public Destroy()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}