using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Validators
{
    public class SetCreditAccount : AbstractValidator<Commands.SetCreditAccount>
    {
        public SetCreditAccount()
        {
            RuleFor(x => x.JournalId).NotEmpty();
        }
    }
}
