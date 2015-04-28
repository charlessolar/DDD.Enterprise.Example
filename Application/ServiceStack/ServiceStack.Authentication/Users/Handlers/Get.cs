using Demo.Application.ServiceStack.Authentication.Users.Responses;
using Demo.Domain.Authentication.Users.Events;
using Demo.Library.Extensions;

using Demo.Library.SSE;
using NServiceBus;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Authentication.Users.Handlers
{
    public class Get :
        IHandleMessages<LoggedIn>,
        IHandleMessages<EmailChanged>,
        IHandleMessages<AvatarChanged>,
        IHandleMessages<NameChanged>,
        IHandleMessages<TimezoneChanged>
    {
        private readonly IDocumentStore _store;
        private readonly ISubscriptionManager _manager;

        public Get(IDocumentStore store, ISubscriptionManager manager)
        {
            _store = store;
            _manager = manager;
        }

        public void Handle(LoggedIn e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.UserId);
                if (get == null)
                {
                    get = new Responses.Get
                    {
                        Id = e.UserId,

                        Name = e.Name,
                        Email = e.Email,
                        NickName = e.NickName,

                        ImageType = e.ImageType,
                        ImageData = e.ImageData
                    };
                    session.Store(get);
                    session.SaveChanges();

                    _manager.Publish(get, ChangeType.NEW);
                }
            }
        }

        public void Handle(EmailChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.UserId);

                get.Email = e.Email;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(AvatarChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.UserId);

                get.ImageType = e.ImageType;
                get.ImageData = e.ImageData;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(NameChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.UserId);

                get.Name = e.Name;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }

        public void Handle(TimezoneChanged e)
        {
            using (var session = _store.OpenSession())
            {
                var get = session.Load<Responses.Get>(e.UserId);

                get.Timezone = e.Timezone;

                session.SaveChanges();

                _manager.Publish(get);
            }
        }
    }
}