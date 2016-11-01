using Demo.Library.Extensions;
using NServiceBus;
using Seed.Attributes;
using Seed.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aggregates.Extensions;
using Commands = Demo.Domain.Accounting.Currency.Commands;
using Type = Seed.Types.Accounting;

namespace Seed.Operations
{
    [Operation("Currency")]
    [Depends("User")]
    [Category("Accounting")]
    public class Currency : IOperation
    {
        public static IEnumerable<Type.Currency> Data = new[] {
            new Type.Currency { Id = Guid.NewGuid(), Code = "USD", Name = "United States Dollar", Symbol = "$", SymbolBefore = true, RoundingFactor = 2, ComputationalAccuracy = 4, Format = "#,###.##", Fraction = "Cents", Activated = true },
        };

        private readonly IMessageSession _bus;

        public Currency(IMessageSession bus)
        {
            _bus = bus;
        }

        public async Task<Boolean> Seed()
        {
            var commands = Data.Select(x => new Commands.Create
            {
                CurrencyId = x.Id,
                Code = x.Code,
                Name = x.Name,
                Symbol = x.Symbol,
                SymbolBefore = x.SymbolBefore,
                RoundingFactor = x.RoundingFactor,
                ComputationalAccuracy = x.ComputationalAccuracy,
                Format = x.Format,
                Fraction = x.Fraction,
            });
            await commands.WhenAllAsync(x => _bus.Command(x));

            var activations = Data.Where(x => x.Activated).Select(x => new Commands.Activate
                {
                    CurrencyId = x.Id,
                });
            await activations.WhenAllAsync(x => _bus.Command(x));

            this.Done = true;
            return this.Done;
        }

        public Boolean Done { get; private set; }
    }
}