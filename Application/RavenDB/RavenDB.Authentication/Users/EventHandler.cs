using Demo.Domain.Authentication.Users.Events;
using NServiceBus;
using Raven.Client;

namespace Demo.Application.RavenDB.Authentication.Users
{
    public class EventHandler :
        IHandleMessages<LoggedIn>,
        IHandleMessages<LoggedOut>,
        IHandleMessages<EmailChanged>,
        IHandleMessages<AvatarChanged>,
        IHandleMessages<NameChanged>
    {
        private readonly IDocumentStore _store;

        public EventHandler(IDocumentStore store)
        {
            _store = store;
        }

        public void Handle(LoggedIn e)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var user = session.Load<User>(e.UserId);
                if (user == null)
                {
                    user = new User
                    {
                        Id = e.UserId,

                        Name = e.Name,
                        Email = e.Email,
                        NickName = e.NickName,

                        ImageType = e.ImageType,
                        ImageData = e.ImageData
                    };
                    session.Store(user);
                }
                session.SaveChanges();
            }
        }

        public void Handle(LoggedOut e)
        {
        }

        public void Handle(EmailChanged e)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var user = session.Load<User>(e.UserId);

                user.Email = e.Email;

                session.Store(user);
                session.SaveChanges();
            }
        }

        public void Handle(AvatarChanged e)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var user = session.Load<User>(e.UserId);

                user.ImageType = e.ImageType;
                user.ImageData = e.ImageData;

                session.Store(user);
                session.SaveChanges();
            }
        }

        public void Handle(NameChanged e)
        {
            using (IDocumentSession session = _store.OpenSession())
            {
                var user = session.Load<User>(e.UserId);

                user.Name = e.Name;

                session.Store(user);
                session.SaveChanges();
            }
        }
    }
}