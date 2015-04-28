using Aggregates.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency
{
    public class Currency : Aggregates.Aggregate<Guid>, ICurrency
    {
        public Aggregates.SingleValueObject<Boolean> Activated { get; private set; }

        private Currency()
        {
            this.Activated = new Aggregates.SingleValueObject<bool>(false);
        }

        public void Create(String Code, String Name, String Symbol, Boolean SymbolBefore, Decimal RoundingFactor, Int32 ComputationalAccuracy, String Format, String Fraction)
        {
            Apply<Events.Created>(e =>
            {
                e.CurrencyId = Id;
                e.Code = Code;
                e.Name = Name;
                e.Symbol = Symbol;
                e.SymbolBefore = SymbolBefore;
                e.RoundingFactor = RoundingFactor;
                e.ComputationalAccuracy = ComputationalAccuracy;
                e.Format = Format;
                e.Fraction = Fraction;
            });
        }

        public void Activate(Authentication.Users.IUser User)
        {
            Apply<Events.Activated>(e =>
            {
                e.CurrencyId = Id;
                e.UserId = User.Id;
            });
        }

        private void Handle(Events.Activated e)
        {
            this.Activated = new Aggregates.SingleValueObject<bool>(true);
        }

        public void AddRate(Decimal Rate, ICurrency Destination, DateTime? EffectiveTill)
        {
            Apply<Events.RateAdded>(e =>
            {
                e.CurrencyId = Id;
                e.Factor = Rate;
                e.DestinationCurrencyId = Destination.Id;
                e.EffectiveTill = EffectiveTill;
            });
        }

        public void ChangeAccuracy(Int32 Accuracy)
        {
            Apply<Events.AccuracyChanged>(e =>
            {
                e.CurrencyId = Id;
                e.ComputationalAccuracy = Accuracy;
            });
        }

        public void ChangeName(String Name)
        {
            Apply<Events.NameChanged>(e =>
            {
                e.CurrencyId = Id;
                e.Name = Name;
            });
        }

        public void ChangeRoundingFactor(Decimal Rounding)
        {
            Apply<Events.RoundingFactorChanged>(e =>
            {
                e.CurrencyId = Id;
                e.RoundingFactor = Rounding;
            });
        }

        public void ChangeSymbol(String Symbol)
        {
            Apply<Events.SymbolChanged>(e =>
            {
                e.CurrencyId = Id;
                e.Symbol = Symbol;
            });
        }

        public void Deactivate(Authentication.Users.IUser User)
        {
            Apply<Events.Deactivated>(e =>
            {
                e.CurrencyId = Id;
                e.UserId = User.Id;
            });
        }

        private void Handle(Events.Deactivated e)
        {
            this.Activated = new Aggregates.SingleValueObject<bool>(false);
        }

        public void ChangeSymbolBefore(Boolean Before)
        {
            Apply<Events.SymbolBefore>(e =>
            {
                e.CurrencyId = Id;
                e.Before = Before;
            });
        }

        public void ChangeFormat(String Format)
        {
            Apply<Events.FormatChanged>(e =>
            {
                e.CurrencyId = Id;
                e.Format = Format;
            });
        }

        public void ChangeFraction(String Fraction)
        {
            Apply<Events.FractionChanged>(e =>
            {
                e.CurrencyId = Id;
                e.Fraction = Fraction;
            });
        }

        public void Destroy()
        {
            if (this.Activated.Value)
                throw new BusinessException("Currency is active");

            Apply<Events.Destroyed>(e =>
            {
                e.CurrencyId = Id;
            });
        }
    }
}