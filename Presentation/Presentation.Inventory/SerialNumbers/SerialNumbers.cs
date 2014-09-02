
using Demo.Library.Queries;
using Demo.Library.Responses;
using Demo.Library.Extensions;
using NServiceBus;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Demo.Presentation.Inventory.Models.SerialNumbers.Services;
using Demo.Presentation.Inventory.Models.SerialNumbers.Responses;

namespace Demo.Presentation.Inventory.SerialNumbers
{
    public class SerialNumbers : Service
    {
        public IBus _bus { get; set; }

        public Task<Full<SerialNumber>> Any(GetSerialNumber request)
        {
            return _bus.Send("application", new Application.Inventory.SerialNumbers.Queries.GetSerialNumber
            {
                Id = request.Id,
            }).Register(x =>
            {
                var result = x.GetQueryResponse<Application.Inventory.SerialNumbers.SerialNumber>();

                if (result == null)
                    throw new HttpError(HttpStatusCode.NotFound, "Get request failed");

                // Convert to our DTO
                var serial = result.ConvertTo<SerialNumber>();


                // Save DTO in cache along with this session Id
                var wrapper = serial.AddSession(base.Cache, Request.GetPermanentSessionId());

                return wrapper.ToResponse();
            });
        }

        public Object Any(FindSerialNumbers request)
        {
            return _bus.Send("application", new Application.Inventory.SerialNumbers.Queries.FindSerialNumbers
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Serial = request.Serial,
                Effective = request.Effective,
                ItemId = request.ItemId,
            }).Register(x =>
            {
                return new Find
                {
                    Results = x.GetQueryListResponse<Application.Inventory.SerialNumbers.SerialNumber>().Select(r => r.ConvertTo<SerialNumber>())
                };
            });
        }

        public Guid Post(CreateSerialNumber request)
        {
            var command = request.ConvertTo<Domain.Inventory.SerialNumbers.Commands.Create>();
            command.ItemId = Guid.NewGuid();
            _bus.Send("domain", command);

            return command.ItemId;
        }
    }
}