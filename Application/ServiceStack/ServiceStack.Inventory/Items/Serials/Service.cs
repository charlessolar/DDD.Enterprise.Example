using Demo.Library.Authentication;
using Demo.Library.Extensions;
using Demo.Library.Services;
using Demo.Application.ServiceStack.Inventory.Models.Items.Serials;
using NServiceBus;
using ServiceStack;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Demo.Application.ServiceStack.Inventory.Items.Serials
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
            return _bus.Send("application", new Application.RavenDB.Inventory.SerialNumbers.Queries.Get
            {
                Id = request.Id,
            }).Register(x =>
            {
                var result = x.GetQueryResponse<Application.RavenDB.Inventory.SerialNumbers.SerialNumber>();

                if (result == null)
                    throw new HttpError(HttpStatusCode.NotFound, "Get request failed");

                // Convert to our DTO
                var serial = result.ConvertTo<GetResponse>();

                return serial;
            });
        }

        public Task<FindResponse> Any(Find request)
        {
            return _bus.Send("application", new Application.RavenDB.Inventory.SerialNumbers.Queries.Find
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Serial = request.Serial,
                Effective = request.Effective,
                ItemId = request.ItemId,
            }).Register(x =>
            {
                return new FindResponse
                {
                    Results = x.GetQueryListResponse<Application.RavenDB.Inventory.SerialNumbers.SerialNumber>().Select(r => r.ConvertTo<GetResponse>())
                };
            });
        }

        public Task Post(Create request)
        {
            var command = request.ConvertTo<Domain.Inventory.Items.SerialNumbers.Commands.Create>();

            return _bus.Send("domain", command).Register();
        }
    }
}