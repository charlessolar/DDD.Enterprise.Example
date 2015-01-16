using Demo.Library.Authentication;
using Demo.Library.Extensions;
using Demo.Library.Responses;
using Demo.Library.Services;
using Demo.Application.ServiceStack.Inventory.Models.Items;
using NServiceBus;
using ServiceStack;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Inventory.Items
{
    [RequireJWT]
    public class Service : DemoService
    {
        private IBus _bus;

        public Service(IBus bus)
        {
            _bus = bus;
        }

        public Task<GetResponse> Any(Get request)
        {
            return _bus.Send("application.ravendb", new Application.RavenDB.Inventory.Items.Queries.Get
            {
                Id = request.Id,
            }).Register(x =>
            {
                var result = x.GetQueryResponse<Application.RavenDB.Inventory.Items.Item>();

                if (result == null)
                    throw new HttpError(HttpStatusCode.NotFound, "Get request failed");

                // Convert application object to our DTO
                var item = result.ConvertTo<GetResponse>();

                return item;
            });
        }

        public Task<FindResponse> Any(Find request)
        {
            return _bus.Send("application.ravendb", new Application.RavenDB.Inventory.Items.Queries.Find
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Number = request.Number,
                Description = request.Description,
            }).Register(x =>
            {
                return new FindResponse
                {
                    Results = x.GetQueryListResponse<Application.RavenDB.Inventory.Items.Item>().Select(r => r.ConvertTo<GetResponse>())
                };
            });
        }

        public Task<Command> Post(Create request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.Commands.Create>();

            return _bus.Send("domain", command).IsCommand<Command>();
        }

        public Task<Command> Post(Description request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.Commands.ChangeDescription>();

            return _bus.Send("domain", command).IsCommand<Command>();
        }
    }
}