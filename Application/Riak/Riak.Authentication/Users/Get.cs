using Demo.Domain.Authentication.Users.Events;
using Demo.Library.Extensions;

using Demo.Library.SSE;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.Library.Updates;
using Demo.Library.Queries;
using Demo.Application.Riak.Infrastructure;
using presentationUsers = Demo.Presentation.ServiceStack.Authentication.Users;
using Aggregates;
using Aggregates.Extensions;

namespace Demo.Application.Riak.Authentication.Users
{
    public class Get :
        IHandleQueries<presentationUsers.Queries.IGet>,
        IHandleMessages<LoggedIn>,
        IHandleMessages<LoggedOut>,
        IHandleMessages<EmailChanged>,
        IHandleMessages<AvatarChanged>,
        IHandleMessages<NameChanged>,
        IHandleMessages<TimezoneChanged>
    {
        
        private readonly IUnitOfWork _uow;

        public Get(IUnitOfWork uow)
        {
           
            _uow = uow;
        }

        public async Task Handle(presentationUsers.Queries.IGet q, IMessageHandlerContext ctx)
        {
            var get = await _uow.Get<presentationUsers.Models.AuUserResponse>(q.UserAuthId);
            ctx.Result(get);
        }

        public async Task Handle(LoggedIn e, IMessageHandlerContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AuUserResponse>(e.UserId);

            get.LastLoginAttempt = DateTime.UtcNow;

            _uow.Save(get);
            ctx.Update(get, ChangeType.Change);
            
        }
        public async Task Handle(LoggedOut e, IMessageHandlerContext ctx)
        {
            
            var user = await _uow.Get<presentationUsers.Models.AuUserResponse>(e.UserId);

            if (user == null) return;
            
        }
        public async Task Handle(EmailChanged e, IMessageHandlerContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AuUserResponse>(e.UserId);

            get.Email = e.Email;

            _uow.Save(get);
            ctx.Update(get, ChangeType.Change);

        }

        public async Task Handle(AvatarChanged e, IMessageHandlerContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AuUserResponse>(e.UserId);

            get.ImageType = e.ImageType;
            get.ImageData = e.ImageData;

            _uow.Save(get);
            ctx.Update(get, ChangeType.Change);

        }

        public async Task Handle(NameChanged e, IMessageHandlerContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AuUserResponse>(e.UserId);

            get.Name = e.Name;

            _uow.Save(get);
            ctx.Update(get, ChangeType.Change);

        }

        public async Task Handle(TimezoneChanged e, IMessageHandlerContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AuUserResponse>(e.UserId);

            get.Timezone = e.Timezone;

            _uow.Save(get);
            ctx.Update(get, ChangeType.Change);

        }
    }
}