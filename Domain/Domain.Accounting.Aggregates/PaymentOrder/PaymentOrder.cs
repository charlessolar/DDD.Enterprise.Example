using Aggregates.Exceptions;
using NServiceBus;
using NServiceBus.Saga;
using ServiceStack;
using Stateless;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.Accounting.PaymentOrder
{
    public class PaymentOrder : Aggregates.Aggregate<Guid>, IPaymentOrder
    {
        public enum TRIGGER
        {
            CONFIRM,
            DISPURSE,
            DISCARD,
            INVALIDATE
        }

        private StateMachine<STATE, TRIGGER> _machine;

        public Aggregates.SingleValueObject<STATE> State { get; private set; }

        private PaymentOrder()
        {
            _machine = new StateMachine<STATE, TRIGGER>(() => this.State.Value, s => this.State = new Aggregates.SingleValueObject<STATE>(s));

            _machine.Configure(STATE.NEW)
                .Permit(TRIGGER.DISCARD, STATE.DISCARDED)
                .Permit(TRIGGER.CONFIRM, STATE.CONFIRMED);

            _machine.Configure(STATE.CONFIRMED)
                .Permit(TRIGGER.DISCARD, STATE.DISCARDED)
                .Permit(TRIGGER.DISPURSE, STATE.DISPURSED)
                .OnEntry(() =>
                {
                    Apply<Events.Confirmed>(e =>
                    {
                        e.PaymentOrderId = Id;
                    });
                });
            _machine.Configure(STATE.DISCARDED)
                .OnEntry(() =>
                {
                    Apply<Events.Discarded>(e =>
                    {
                        e.PaymentOrderId = Id;
                    });
                });
            _machine.Configure(STATE.DISPURSED)
                .OnEntry(() =>
                {
                    Apply<Events.Dispursed>(e =>
                    {
                        e.PaymentOrderId = Id;
                    });
                });

            _machine.Configure(STATE.INVALID)
                .Permit(TRIGGER.INVALIDATE, STATE.INVALID)
                .OnEntry(() =>
                {
                    Apply<Events.Invalidated>(e =>
                    {
                        e.PaymentOrderId = Id;
                    });
                });
            _machine.OnUnhandledTrigger((s, t) =>
            {
                throw new BusinessException("Invalid state change: trigger {0} on state {1}".Fmt(t, s));
            });
        }

        public void ChangeEffective(DateTime Effective)
        {
            Apply<Events.EffectiveChanged>(e =>
            {
                e.PaymentOrderId = Id;
                e.Effective = Effective;
            });
        }

        public void ChangeMethod(Guid MethodId)
        {
            Apply<Events.MethodChanged>(e =>
            {
                e.PaymentOrderId = Id;
                e.MethodId = MethodId;
            });
        }

        public void ChangeReference(String Reference)
        {
            Apply<Events.ReferenceChanged>(e =>
            {
                e.PaymentOrderId = Id;
                e.Reference = Reference;
            });
        }

        public void Confirm()
        {
            _machine.Fire(TRIGGER.CONFIRM);
        }

        private void Handle(Events.Confirmed e)
        {
            this.State = new Aggregates.SingleValueObject<STATE>(STATE.CONFIRMED);
        }

        public void Discard()
        {
            _machine.Fire(TRIGGER.DISCARD);
        }

        private void Handle(Events.Discarded e)
        {
            this.State = new Aggregates.SingleValueObject<STATE>(STATE.DISCARDED);
        }

        public void Dispurse()
        {
            _machine.Fire(TRIGGER.DISPURSE);
        }

        private void Handle(Events.Dispursed e)
        {
            this.State = new Aggregates.SingleValueObject<STATE>(STATE.DISPURSED);
        }

        public void Start(String Identity)
        {
            Apply<Events.Started>(e =>
            {
                e.PaymentOrderId = Id;
                e.Identity = Identity;
            });
        }

        private void Handle(Events.Started e)
        {
            this.State = new Aggregates.SingleValueObject<STATE>(STATE.NEW);
        }
    }
}