using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Item.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Effective).NotEmpty();
            RuleFor(x => x.Reference).NotEmpty();
            RuleFor(x => x.AccountId).NotEmpty();
            RuleFor(x => x.PeriodId).NotEmpty();
        }
    }
}