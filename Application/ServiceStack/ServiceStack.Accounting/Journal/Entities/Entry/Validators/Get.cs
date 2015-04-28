using ServiceStack.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Accounting.Journal.Entities.Entry.Validators
{
    public class Index : AbstractValidator<Services.Index>
    {
        public Index()
        {
            RuleFor(x => x.JournalId).NotEmpty();
        }
    }
}