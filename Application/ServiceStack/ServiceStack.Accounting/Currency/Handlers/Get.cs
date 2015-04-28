using Demo.Domain.Accounting.Currency.Events;
using Demo.Library.Extensions;
using Demo.Library.SSE;
using NServiceBus;
using Raven.Client;

namespace Demo.Application.ServiceStack.Accounting.Currency.Handlers
{
    public class Get :
        IHandleMessages<AccuracyChanged>,
        IHandleMessages<Activated>,
        IHandleMessages<Created>,
        IHandleMessages<Deactivated>,
        IHandleMessages<RoundingFactorChanged>,
        IHandleMessages<SymbolBefore>,
        IHandleMessages<SymbolChanged>,
        IHandleMessages<FormatChanged>,
        IHandleMessages<FractionChanged>
    {
        private readonly IDocumentStore _store;
        private readonly ISubscriptionManager _manager;

        public Get(IDocumentStore store, ISubscriptionManager manager)
        {
            _store = store;
            _manager = manager;
        }

        public void Handle(AccuracyChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.ComputationalAccuracy = e.ComputationalAccuracy;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(Activated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.Activated = true;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var get = new Responses.Get
                {
                    Id = e.CurrencyId,
                    Code = e.Code,
                    Name = e.Name,
                    Symbol = e.Symbol,
                    SymbolBefore = e.SymbolBefore,
                    RoundingFactor = e.RoundingFactor,
                    ComputationalAccuracy = e.ComputationalAccuracy,
                    Format = e.Format,
                    Fraction = e.Fraction,
                };

                session.Store(get);
                session.SaveChanges();

                _manager.Publish(get, ChangeType.NEW);
            }
        }

        public void Handle(Deactivated e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.Activated = false;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.Name = e.Name;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(FormatChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.Format = e.Format;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(FractionChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.Fraction = e.Fraction;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(RoundingFactorChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.RoundingFactor = e.RoundingFactor;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(SymbolBefore e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.SymbolBefore = e.Before;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(SymbolChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.CurrencyId);
                get.Symbol = e.Symbol;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }
    }
}