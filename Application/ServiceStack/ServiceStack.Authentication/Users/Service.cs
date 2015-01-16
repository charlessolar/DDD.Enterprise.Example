using Forte.Application.ServiceStack.Authentication.Models.Users;
using Forte.Library.Authentication;
using Forte.Library.Extensions;
using Forte.Library.Responses;
using Forte.Library.Services;
using NServiceBus;
using ServiceStack;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Forte.Application.ServiceStack.Authentication.Users
{
    [RequireJWT]
    public class Service : ForteService
    {
        private IBus _bus;

        public Service(IBus bus)
        {
            _bus = bus;
        }

        public Task<GetResponse> Any(Get request)
        {
            return _bus.Send("application.ravendb", new Forte.Application.RavenDB.Authentication.Users.Queries.Get
            {
                Id = Profile.UserId
            }).Register(x =>
            {
                var result = x.GetQueryResponse<Forte.Application.RavenDB.Authentication.Users.User>();

                if (result == null)
                    throw new HttpError(HttpStatusCode.NotFound, "Get request failed");

                // Convert application object to our DTO
                var item = result.ConvertTo<GetResponse>();

                return item;
            });
        }

        public async Task<Command> Post(Login request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Login>();

            var image = await GetImageFromUrl(request.ImageUrl);

            command.ImageType = image.Type;
            command.ImageData = image.Data;

            return await _bus.Send("domain", command).IsCommand<Command>();
        }

        public Task<Command> Post(Logout request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Logout>();

            command.UserId = Profile.UserId;

            return _bus.Send("domain", command).IsCommand<Command>();
        }

        public Task<Command> Post(ChangeEmail request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeEmail>();

            command.UserId = Profile.UserId;

            return _bus.Send("domain", command).IsCommand<Command>();
        }

        public Task<Command> Post(ChangeAvatar request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeAvatar>();

            command.UserId = Profile.UserId;

            return _bus.Send("domain", command).IsCommand<Command>();
        }

        public Task<Command> Post(ChangeName request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeName>();

            command.UserId = Profile.UserId;

            return _bus.Send("domain", command).IsCommand<Command>();
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