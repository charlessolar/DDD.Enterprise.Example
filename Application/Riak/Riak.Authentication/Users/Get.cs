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
using Demo.Presentation.ServiceStack.Infrastructure.SSE;
using Demo.Library.Queries;
using Demo.Application.Riak.Infrastructure;
using presentationUsers = Demo.Presentation.ServiceStack.Authentication.Users;
using Aggregates;
using Aggregates.Extensions;

namespace Demo.Application.Riak.Authentication.Users
{
    public class Get :
        IHandleQueries<presentationUsers.Queries.Get>,
        IHandleMessagesAsync<Registered>,
        IHandleMessagesAsync<LoggedIn>,
        IHandleMessagesAsync<LoggedOut>,
        IHandleMessagesAsync<PasswordChanged>,
        IHandleMessagesAsync<EmailChanged>,
        IHandleMessagesAsync<AvatarChanged>,
        IHandleMessagesAsync<NameChanged>,
        IHandleMessagesAsync<TimezoneChanged>,
        IHandleMessagesAsync<Deactivated>,
        IHandleMessagesAsync<RolesAssigned>,
        IHandleMessagesAsync<RolesUnassigned>,
        IHandleMessagesAsync<Updated>
    {
        private readonly IBus _bus;
        private readonly IUnitOfWork _uow;

        public Get(IBus bus, IUnitOfWork uow)
        {
            _bus = bus;
            _uow = uow;
        }

        public async Task Handle(presentationUsers.Queries.Get q, IHandleContext ctx)
        {
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(q.UserAuthId);
            ctx.ResultAsync(get);
        }
        public Task Handle(Registered e, IHandleContext ctx)
        {
            var get = new presentationUsers.Models.AU_UserResponse
            {
                Id = e.UserAuthId,
                Password = e.Password
            };
            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.NEW);

            return Task.FromResult(0);
        }

        public async Task Handle(LoggedIn e, IHandleContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserId);

            get.LastLoginAttempt = DateTime.UtcNow;

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);

            _Demo.Report("Login", new { Name = get.Name, Id = get.Id, Email = get.Email });
        }
        public async Task Handle(LoggedOut e, IHandleContext ctx)
        {
            
            var user = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserId);

            if (user == null) return;

            _Demo.Report("Logout", new { Name = user.Name, Id = user.Id, Email = user.Email });


        }
        public async Task Handle(PasswordChanged e, IHandleContext ctx)
        {
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserId);

            get.Password = e.Password;

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);
        }
        public async Task Handle(EmailChanged e, IHandleContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserId);

            get.Email = e.Email;

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);

        }

        public async Task Handle(AvatarChanged e, IHandleContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserId);

            get.ImageType = e.ImageType;
            get.ImageData = e.ImageData;

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);

        }

        public async Task Handle(NameChanged e, IHandleContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserId);

            get.Name = e.Name;

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);

        }

        public async Task Handle(TimezoneChanged e, IHandleContext ctx)
        {
            
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserId);

            get.Timezone = e.Timezone;

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);

        }
        public async Task Handle(Deactivated e, IHandleContext ctx)
        {
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserAuthId);
            get.LockedDate = DateTime.UtcNow;

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);
        }

        public async Task Handle(RolesAssigned e, IHandleContext ctx) {

            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserAuthId);

            get.Roles = get.Roles.Concat(e.Roles).Distinct();

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);
        }

        public async Task Handle(RolesUnassigned e, IHandleContext ctx)
        {
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserAuthId);

            get.Roles = get.Roles.Except(e.Roles).Distinct();

            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);
        }
        public async Task Handle(Updated e, IHandleContext ctx)
        {
            var get = await _uow.Get<presentationUsers.Models.AU_UserResponse>(e.UserAuthId);

            get.Name = e.DisplayName;
            get.Email = e.PrimaryEmail;
            get.Nickname = e.Nickname;
            get.Timezone = e.Timezone;
            get.ModifiedDate = DateTime.UtcNow;
            
            _uow.Save(get);
            ctx.UpdateAsync(get, ChangeType.CHANGE);
        }
    }
}