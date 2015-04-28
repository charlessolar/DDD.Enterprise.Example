using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Validators
{
    public class Reconcile : AbstractValidator<Commands.Reconcile>
    {
        public Reconcile()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.OtherItemId).NotEmpty();
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}