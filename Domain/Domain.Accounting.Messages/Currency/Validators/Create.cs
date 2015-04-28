using FluentValidation;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency.Validators
{
    public class Create : AbstractValidator<Commands.Create>
    {
        public Create()
        {
            RuleFor(x => x.CurrencyId).NotEmpty();
            RuleFor(x => x.Code).NotEmpty().Length(2, 8);
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Symbol).NotEmpty().Length(1);
            RuleFor(x => x.RoundingFactor).GreaterThanOrEqualTo(0);
            RuleFor(x => x.ComputationalAccuracy).GreaterThanOrEqualTo(0);
        }
        public Guid CurrencyId { get; set; }

        public String Code { get; set; }
        public String Name { get; set; }
        public String Symbol { get; set; }

        public Boolean SymbolAfter { get; set; }
        public Decimal RoundingFactor { get; set; }
        public Int32 ComputationalAccuracy { get; set; }

        public Boolean Base { get; set; }
    }
}
