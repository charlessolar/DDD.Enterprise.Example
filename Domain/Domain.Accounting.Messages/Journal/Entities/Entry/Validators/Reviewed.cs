using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Journal.Entities.Entry.Validators
{
    public class Reviewed : AbstractValidator<Commands.Reviewed>
    {
        public Reviewed()
        {
            RuleFor(x => x.JournalId).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.EntryId).NotEmpty();
        }
    }
}