using NServiceBus;
using ServiceStack;
using System;
using System.Net;
using System.Threading.Tasks;
using Demo.Presentation.ServiceStack.Authentication.Users.Models;
using Demo.Presentation.ServiceStack.Infrastructure.Services;
using Demo.Presentation.ServiceStack.Infrastructure.Extensions;
using IGet = Demo.Presentation.ServiceStack.Authentication.Users.Queries.IGet;

namespace Demo.Presentation.ServiceStack.Authentication.Users
{
    public class Service : DemoService
    {
        private readonly IMessageSession _bus;

        public Service(IMessageSession bus)
        {
            _bus = bus;
        }
        
        public async Task<object> Any(Models.AuGet request)
        {
            return await _bus.RequestToRiak<IGet, AuUserResponse>(x =>
            {
                x.UserAuthId = request.UserAuthId;
            }, request).ConfigureAwait(false);
        }
        

        public async Task Post(Models.AuLogin request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Login>();

            await _bus.CommandToDomain(command).ConfigureAwait(false);
        }

        public async Task Post(Models.AuLogout request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Logout>();
            

            await _bus.CommandToDomain(command).ConfigureAwait(false);
        }
        
        public async Task Post(Models.AuChangeEmail request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeEmail>();
            

            await _bus.CommandToDomain(command).ConfigureAwait(false);
        }
        
        public async Task Post(Models.AuChangeAvatar request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeAvatar>();
            

            await _bus.CommandToDomain(command).ConfigureAwait(false);
        }
        
        public async Task Post(Models.AuChangeName request)
        {
            var session = Request.GetSessionId();

            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeName>();
            

            await _bus.CommandToDomain(command).ConfigureAwait(false);
        }
        public Task Post(Models.AuChangePassword request)
        {
            //var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangePassword>();

            //await _bus.CommandToDomain(command).ConfigureAwait(false);
            return Task.CompletedTask;
        }
        public Task Post(Models.AuAssignRoles request)
        {
            //var command = request.ConvertTo<Domain.Authentication.Users.Commands.AssignRoles>();

            //await _bus.CommandToDomain(command).ConfigureAwait(false);
            return Task.CompletedTask;
        }
        public Task Post(Models.AuUnassignRoles request)
        {
            //var command = request.ConvertTo<Domain.Authentication.Users.Commands.UnassignRoles>();

            //await _bus.CommandToDomain(command).ConfigureAwait(false);
            return Task.CompletedTask;
        }


        public async Task Post(Models.AuChangeTimezone request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeTimezone>();
            

            await _bus.CommandToDomain(command).ConfigureAwait(false);
        }

        private class Image
        {
            public string Type { get; set; }

            public string Data { get; set; }
        }

        private async Task<Image> GetImageFromUrl(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                var response = await request.GetResponseAsync().ConfigureAwait(false);

                var buffer = response.GetResponseStream().ReadFully();
                response.Close();

                return new Image { Type = response.ContentType, Data = Convert.ToBase64String(buffer) };
            }
            catch
            {
                return null;
            }
        }
    }
}