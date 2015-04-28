using Demo.Domain.Accounting.Account.Events;
using Demo.Library.Extensions;
using Demo.Library.SSE;
using Nest;
using NServiceBus;
using Raven.Client;
using System.Linq;

namespace Demo.Application.ServiceStack.Accounting.Account.Handlers
{
    public class Get :
        IHandleMessages<Created>,
        IHandleMessages<DescriptionChanged>,
        IHandleMessages<Destroyed>,
        IHandleMessages<NameChanged>,
        IHandleMessages<ParentChanged>,
        IHandleMessages<TypeChanged>,
        IHandleMessages<Frozen>,
        IHandleMessages<Unfrozen>,
        IHandleMessages<Demo.Domain.Configuration.AccountType.Events.NameChanged>
    {
        private readonly IDocumentStore _store;
        private readonly ISubscriptionManager _manager;
        private readonly IElasticClient _elastic;

        public Get(IDocumentStore store, IElasticClient elastic, ISubscriptionManager manager)
        {
            _store = store;
            _manager = manager;
            _elastic = elastic;
        }

        public void Handle(Created e)
        {
            using (var session = _store.OpenSession())
            {
                var currency = _elastic.Get<Currency.Responses.Index>(e.CurrencyId);

                var get = new Responses.Get
                {
                    Id = e.AccountId,
                    Code = e.Code,
                    Name = e.Name,
                    Operation = e.Operation,
                    AcceptPayments = e.AcceptPayments,
                    AllowReconcile = e.AllowReconcile,
                    CurrencyId = currency.Source.Id,
                    Currency = currency.Source.Code
                };

                session.Store(get);
                session.SaveChanges();

                _manager.Publish(get, ChangeType.NEW);
            }
        }

        public void Handle(Demo.Domain.Configuration.AccountType.Events.NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var gets = session.Query<Responses.Get>().Where(x => x.TypeId == e.AccountTypeId).ToList();

                foreach (var get in gets)
                    get.Type = e.Name;

                session.SaveChanges();

                _manager.PublishAll(gets);
            }
        }

        public void Handle(DescriptionChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.AccountId);
                get.Description = e.Description;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(Destroyed e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.AccountId);

                session.Delete(get);
                session.SaveChanges();

                _manager.Publish(get, ChangeType.DELETE);
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.AccountId);
                get.Name = e.Name;

                var gets = session.Query<Responses.Get>().Where(x => x.ParentId == e.AccountId).ToList();

                foreach (var g in gets)
                    g.Parent = e.Name;

                session.SaveChanges();

                _manager.PublishAll(gets);
                _manager.Publish(get);
            }
        }

        public void Handle(TypeChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.AccountId);

                if (e.TypeId.HasValue)
                {
                    var type = _elastic.Get<Configuration.AccountType.Responses.Index>(e.TypeId.Value);

                    get.TypeId = e.TypeId;
                    get.Type = type.Source.Name;
                }
                else
                {
                    get.TypeId = null;
                    get.Type = "";
                }

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(ParentChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.AccountId);

                if (e.ParentId.HasValue)
                {
                    var parent = session.Load<Responses.Get>(e.ParentId);
                    get.ParentId = e.ParentId;
                    get.Parent = parent.Name;

                    var parentCode = parent.Code;
                    while (parent.ParentId.HasValue)
                    {
                        parent = session.Load<Responses.Get>(parent.ParentId);
                        parentCode = parent.Code + parentCode;
                    }

                    get.ParentCode = parentCode;
                }
                else
                {
                    get.ParentId = null;
                    get.ParentCode = "";
                    get.Parent = "";
                }

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(Frozen e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.AccountId);
                get.Frozen = true;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(Unfrozen e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.AccountId);
                get.Frozen = false;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }
    }
}