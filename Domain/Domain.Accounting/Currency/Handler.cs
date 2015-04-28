using Aggregates;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.Currency
{
    public class Handler :
        IHandleMessages<Commands.Activate>,
        IHandleMessages<Commands.AddRate>,
        IHandleMessages<Commands.ChangeAccuracy>,
        IHandleMessages<Commands.ChangeName>,
        IHandleMessages<Commands.ChangeRoundingFactor>,
        IHandleMessages<Commands.ChangeSymbol>,
        IHandleMessages<Commands.Create>,
        IHandleMessages<Commands.Deactivate>,
        IHandleMessages<Commands.SymbolBefore>,
        IHandleMessages<Commands.ChangeFormat>,
        IHandleMessages<Commands.ChangeFraction>
    {
        private readonly IUnitOfWork _uow;
        private readonly IBus _bus;

        public Handler(IUnitOfWork uow, IBus bus)
        {
            _uow = uow;
            _bus = bus;
        }

        public void Handle(Commands.Create command)
        {
            var user = _uow.R<Authentication.Users.User>().Get(command.UserId);
            var currency = _uow.R<Currency>().New(command.CurrencyId);
            currency.Create(command.Code, command.Name, command.Symbol, command.SymbolBefore, command.RoundingFactor, command.ComputationalAccuracy, command.Format, command.Fraction);
        }

        public void Handle(Commands.Activate command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            var user = _uow.R<Authentication.Users.User>().Get(command.UserId);
            currency.Activate(user);
        }

        public void Handle(Commands.AddRate command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            var destination = _uow.R<Currency>().Get(command.DestinationCurrencyId);
            currency.AddRate(command.Factor, destination, command.EffectiveTill);
        }

        public void Handle(Commands.ChangeFormat command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            currency.ChangeFormat(command.Format);
        }

        public void Handle(Commands.ChangeFraction command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            currency.ChangeFraction(command.Fraction);
        }

        public void Handle(Commands.ChangeAccuracy command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            currency.ChangeAccuracy(command.ComputationalAccuracy);
        }

        public void Handle(Commands.ChangeName command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            currency.ChangeName(command.Name);
        }

        public void Handle(Commands.ChangeRoundingFactor command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            currency.ChangeRoundingFactor(command.RoundingFactor);
        }

        public void Handle(Commands.ChangeSymbol command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            currency.ChangeSymbol(command.Symbol);
        }

        public void Handle(Commands.Deactivate command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            var user = _uow.R<Authentication.Users.User>().Get(command.UserId);
            currency.Deactivate(user);
        }

        public void Handle(Commands.SymbolBefore command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            currency.ChangeSymbolBefore(command.Before);
        }

        public void Handle(Commands.Destroy command)
        {
            var currency = _uow.R<Currency>().Get(command.CurrencyId);
            currency.Destroy();
        }
    }
}