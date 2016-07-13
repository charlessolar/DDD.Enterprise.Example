
using Demo.Library.Extensions;
using Demo.Library.SSE;
using NServiceBus;
using ServiceStack;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Demo.Presentation.ServiceStack.Infrastructure.Services;
using Demo.Presentation.ServiceStack.Infrastructure.Authentication;
using Demo.Presentation.ServiceStack.Infrastructure.Responses;
using Demo.Presentation.ServiceStack.Infrastructure.Extensions;
using ServiceStack.Auth;

namespace Demo.Presentation.ServiceStack.Authentication.Users
{
    public class Service : DemoService
    {
        private readonly IBus _bus;

        public Service(IBus bus)
        {
            _bus = bus;
        }
        
        public async Task<Object> Any(Models.AU_Get request)
        {
            return await _bus.SendToRiak<Queries.Get>(x =>
            {
                x.UserAuthId = request.UserAuthId;
            }).AsQueryResult(request);
        }

        public async Task Post(Models.AU_Register request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Register>();

            await _bus.CommandToDomain(command);
        }

        public async Task Post(Models.AU_Update request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Update>();

            await _bus.CommandToDomain(command);
        }

        public async Task Post(Models.AU_Login request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Login>();

            await _bus.CommandToDomain(command);
        }

        public async Task Post(Models.AU_Logout request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Logout>();
            

            await _bus.CommandToDomain(command);
        }
        
        public async Task Post(Models.AU_ChangeEmail request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeEmail>();
            

            await _bus.CommandToDomain(command);
        }
        
        public async Task Post(Models.AU_ChangeAvatar request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeAvatar>();
            

            await _bus.CommandToDomain(command);
        }
        
        public async Task Post(Models.AU_ChangeName request)
        {
            var session = Request.GetSessionId();

            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeName>();
            

            await _bus.CommandToDomain(command);
        }
        public async Task Post(Models.AU_ChangePassword request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangePassword>();

            await _bus.CommandToDomain(command);
        }
        public async Task Post(Models.AU_AssignRoles request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.AssignRoles>();

            await _bus.CommandToDomain(command);
        }
        public async Task Post(Models.AU_UnassignRoles request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.UnassignRoles>();

            await _bus.CommandToDomain(command);
        }


        public async Task Post(Models.AU_ChangeTimezone request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeTimezone>();
            

            await _bus.CommandToDomain(command);
        }

        private class Image
        {
            public String Type { get; set; }

            public String Data { get; set; }
        }

        private async Task<Image> GetImageFromUrl(String Url)
        {
            try
            {
                var request = WebRequest.Create(Url);
                var response = await request.GetResponseAsync();

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