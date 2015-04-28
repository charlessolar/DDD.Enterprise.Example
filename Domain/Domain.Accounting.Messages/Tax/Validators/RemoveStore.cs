using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Tax.Validators
{
    public class RemoveStore : AbstractValidator<Commands.RemoveStore>
    {
        public RemoveStore()
        {
            RuleFor(x => x.TaxId).NotEmpty();
            RuleFor(x => x.StoreId).NotEmpty();
        }
    }
}