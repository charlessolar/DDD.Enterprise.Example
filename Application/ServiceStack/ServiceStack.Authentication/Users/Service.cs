using Demo.Library.Authentication;
using Demo.Library.Extensions;
using Demo.Library.Queries.Processor;
using Demo.Library.Responses;
using Demo.Library.Services;
using Demo.Library.SSE;
using NServiceBus;
using Raven.Client;
using ServiceStack;
using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Authentication.Users
{
    [RequireJWT]
    public class Service : DemoService
    {
        private readonly IBus _bus;
        private readonly IQueryProcessor _processor;
        private readonly ISubscriptionManager _manager;
        private readonly IDocumentStore _store;

        public Service(IBus bus, IQueryProcessor processor, ISubscriptionManager manager, IDocumentStore store)
        {
            _bus = bus;
            _processor = processor;
            _manager = manager;
            _store = store;
        }

        public Object Any(Services.Get request)
        {
            return base.Request.ToOptimizedCachedAndSubscribedResult(request, base.Cache, _manager, () =>
            {
                var dto = _store.WaitAndLoad<Responses.Get>(Profile.UserId);
                return dto.ToQueryResponse(request);
            });
        }

        public async Task<Object> Post(Services.Login request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Login>();

            var image = await GetImageFromUrl(request.ImageUrl);

            command.ImageType = image.Type;
            command.ImageData = image.Data;

            return await _bus.Send(command).IsCommand<Command>();
        }

        public async Task<Object> Post(Services.Logout request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.Logout>();

            command.UserId = Profile.UserId;

            return await _bus.Send(command).IsCommand<Command>();
        }

        public async Task<Object> Post(Services.ChangeEmail request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeEmail>();

            command.UserId = Profile.UserId;

            return await _bus.Send(command).IsCommand<Command>();
        }

        public async Task<Object> Post(Services.ChangeAvatar request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeAvatar>();

            command.UserId = Profile.UserId;

            return await _bus.Send(command).IsCommand<Command>();
        }

        public async Task<Object> Post(Services.ChangeName request)
        {
            var session = Request.GetPermanentSessionId();

            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeName>();

            command.UserId = Profile.UserId;

            return await _bus.Send(command).IsCommand<Command>();
        }

        public async Task<Object> Post(Services.ChangeTimezone request)
        {
            var command = request.ConvertTo<Domain.Authentication.Users.Commands.ChangeTimezone>();

            command.UserId = Profile.UserId;

            return await _bus.Send(command).IsCommand<Command>();
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