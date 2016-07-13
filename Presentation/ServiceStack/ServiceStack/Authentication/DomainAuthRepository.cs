using ServiceStack.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack;
using NServiceBus;
using Aggregates.Extensions;
using Demo.Domain.Authentication.Users.Commands;
using Demo.Presentation.ServiceStack.Infrastructure.Extensions;
using Demo.Presentation.ServiceStack.Authentication.Users.Queries;
using Demo.Presentation.ServiceStack.Authentication.Users.Models;
using NLog;
using System.Threading.Tasks;

namespace Demo.Presentation.ServiceStack.Authentication
{

    public class DomainAuthRepository : IUserAuthRepository, IManageRoles
    {
        private static ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IBus _bus;

        public DomainAuthRepository(IBus bus)
        {
            _bus = bus;
        }

        public void AssignRoles(string userAuthId, ICollection<string> roles = null, ICollection<string> permissions = null)
        {
            _bus.CommandToDomain<Domain.Authentication.Users.Commands.AssignRoles>(x =>
            {
                x.UserAuthId = userAuthId;
                x.Roles = roles;
                x.Permissions = permissions;
            }).Wait();
        }

        public IUserAuthDetails CreateOrMergeAuthSession(IAuthSession authSession, IAuthTokens tokens)
        {
            var userAuth = GetUserAuth(authSession, tokens);
            var details = new UserAuthDetails { Provider = tokens.Provider, UserId = tokens.UserId };

            details.PopulateMissing(tokens, overwriteReserved: true);
            userAuth.PopulateMissingExtended(details);

            userAuth.ModifiedDate = DateTime.UtcNow;
            if (userAuth.CreatedDate == default(DateTime))
                userAuth.CreatedDate = userAuth.ModifiedDate;

            UpdateUserAuth(userAuth, userAuth);

            details.UserAuthId = userAuth.Id;

            details.ModifiedDate = userAuth.ModifiedDate;
            if (details.CreatedDate == default(DateTime))
                details.CreatedDate = userAuth.ModifiedDate;

            // Save..

            return details;
        }

        public IUserAuth CreateUserAuth(IUserAuth newUser, string password)
        {
            newUser.ValidateNewUser(password);

            var command = new Domain.Authentication.Users.Commands.Register
            {
                UserAuthId = newUser.UserName,
                Password = password
            };

            _bus.CommandToDomain(command).Wait();

            return newUser;
        }

        public void DeleteUserAuth(string userAuthId)
        {
            _bus.CommandToDomain<Deactivate>(x =>
            {
                x.UserAuthId = userAuthId;
            }).Wait();
        }

        public ICollection<string> GetPermissions(string userAuthId)
        {
            var user = _bus.SendToRiak<Get>(x =>
            {
                x.UserAuthId = userAuthId;
            }).AsSyncQueryResult<AU_UserResponse>();

            return user?.Payload?.Permissions?.ToList();
        }

        public ICollection<string> GetRoles(string userAuthId)
        {
            var user = _bus.SendToRiak<Get>(x =>
            {
                x.UserAuthId = userAuthId;
            }).AsSyncQueryResult<AU_UserResponse>();

            return user?.Payload?.Roles?.ToList();
        }

        public IUserAuth GetUserAuth(string userAuthId)
        {
            var user = _bus.SendToRiak<Get>(x =>
            {
                x.UserAuthId = userAuthId;
            }).AsSyncQueryResult<AU_UserResponse>();
            
            if (user == null || user.Payload == null)
                return null;

            var payload = user.Payload;
            var userAuth = new UserAuth
            {
                DisplayName = payload.Name,
                PrimaryEmail = payload.Email,
                Nickname = payload.Nickname,
                TimeZone = payload.Timezone,
                CreatedDate = payload.CreatedDate,
                ModifiedDate = payload.ModifiedDate,
                PasswordHash = payload.Password,
                InvalidLoginAttempts = payload.InvalidLoginAttempts,
                LastLoginAttempt = payload.LastLoginAttempt,
                LockedDate = payload.LockedDate,
                Permissions = payload.Permissions?.ToList(),
                Roles = payload.Roles?.ToList(),
            };

            return userAuth;
        }

        public IUserAuth GetUserAuth(IAuthSession authSession, IAuthTokens tokens)
        {
            return GetUserAuth(authSession.UserAuthName);
        }

        public IUserAuth GetUserAuthByUserName(string userNameOrEmail)
        {
            return GetUserAuth(userNameOrEmail);
        }

        public List<IUserAuthDetails> GetUserAuthDetails(string userAuthId)
        {
            return new List<IUserAuthDetails>();
        }

        public bool HasPermission(string userAuthId, string permission)
        {
            var user = _bus.SendToRiak<Get>(x =>
            {
                x.UserAuthId = userAuthId;
            }).AsSyncQueryResult<AU_UserResponse>();

            return user?.Payload?.Permissions?.Contains(permission) ?? false;
        }

        public bool HasRole(string userAuthId, string role)
        {
            var user = _bus.SendToRiak<Get>(x =>
            {
                x.UserAuthId = userAuthId;
            }).AsSyncQueryResult<AU_UserResponse>();

            return user?.Payload?.Roles?.Contains(role) ?? false;
        }

        public void LoadUserAuth(IAuthSession session, IAuthTokens tokens)
        {
            var userAuth = GetUserAuth(session, tokens);
            LoadUserAuth(session, userAuth);
        }

        private void LoadUserAuth(IAuthSession session, IUserAuth userAuth)
        {
            session.PopulateSession(userAuth,
                GetUserAuthDetails(session.UserAuthId).ConvertAll(x => (IAuthTokens)x));
        }

        public void SaveUserAuth(IUserAuth userAuth)
        {
            var user = _bus.SendToRiak<Get>(x =>
            {
                x.UserAuthId = userAuth.UserName;
            }).AsSyncQueryResult<AU_UserResponse>();

            var payload = user.Payload;

            _bus.CommandToDomain<Update>(x =>
            {
                x.UserAuthId = userAuth.UserName;
                x.DisplayName = userAuth.DisplayName;
                x.Nickname = userAuth.Nickname;
                x.Timezone = userAuth.TimeZone;
                x.PrimaryEmail = userAuth.PrimaryEmail;
            }).Wait();
        }

        public void SaveUserAuth(IAuthSession authSession)
        {
            var user = _bus.SendToRiak<Get>(x =>
            {
                x.UserAuthId = authSession.UserName;
            }).AsSyncQueryResult<AU_UserResponse>();

            var payload = user.Payload;

            _bus.CommandToDomain<Update>(x =>
            {
                x.UserAuthId = authSession.UserName;
                x.DisplayName = authSession.DisplayName;
                x.PrimaryEmail = authSession.Email;
            }).Wait();
        }

        public bool TryAuthenticate(string userName, string password, out IUserAuth userAuth)
        {

            try
            {
                _bus.CommandToDomain<Login>(e =>
                {
                    e.UserId = userName;
                    e.Password = password;
                }).Wait();

                userAuth = GetUserAuthByUserName(userName);
            }
            catch
            {
                userAuth = null;
                this.RecordInvalidLoginAttempt(userAuth);
                return false;
            }
            this.RecordSuccessfulLogin(userAuth);
            return true;
        }

        public bool TryAuthenticate(Dictionary<string, string> digestHeaders, string privateKey, int nonceTimeOut, string sequence, out IUserAuth userAuth)
        {
            throw new NotImplementedException();
        }

        public void UnAssignRoles(string userAuthId, ICollection<string> roles = null, ICollection<string> permissions = null)
        {
            var userAuth = GetUserAuthByUserName(userAuthId);
            _bus.CommandToDomain<Demo.Domain.Authentication.Users.Commands.UnassignRoles>(x =>
            {
                x.UserAuthId = userAuthId;
                x.Roles = roles;
                x.Permissions = permissions;
            }).Wait();
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUser, IUserAuth newUser)
        {
            var command = new Domain.Authentication.Users.Commands.Update
            {
                UserAuthId = existingUser.UserName,
                PrimaryEmail = newUser.PrimaryEmail,
                Nickname = newUser.Nickname,
                DisplayName = newUser.DisplayName,
                Timezone = newUser.TimeZone
            };
            _bus.CommandToDomain(command).Wait();

            existingUser.PrimaryEmail = newUser.PrimaryEmail;
            existingUser.Nickname = newUser.Nickname;
            existingUser.DisplayName = newUser.DisplayName;
            existingUser.TimeZone = newUser.TimeZone;

            return existingUser;
        }

        public IUserAuth UpdateUserAuth(IUserAuth existingUser, IUserAuth newUser, string password)
        {

            var command = new Domain.Authentication.Users.Commands.ChangePassword
            {
                UserAuthId = existingUser.UserName,
                Password = password
            };

            _bus.CommandToDomain(command).Wait();


            var command2 = new Domain.Authentication.Users.Commands.Update
            {
                UserAuthId = existingUser.UserName,
                DisplayName = newUser.DisplayName,
                Nickname = newUser.Nickname,
                PrimaryEmail = newUser.PrimaryEmail,
                Timezone = newUser.TimeZone,
            };

            _bus.CommandToDomain(command2).Wait();

            return existingUser;
        }
    }
}