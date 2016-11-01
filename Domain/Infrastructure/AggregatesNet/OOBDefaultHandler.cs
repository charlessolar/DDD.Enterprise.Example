namespace Demo.Domain.Infrastructure.AggregatesNet
{
    //public class OOBDefaultHandler : IOOBHandler
    //{
    //    private static NLog.ILogger Logger = LogManager.GetCurrentClassLogger();
    //    private readonly IStoreEvents _store;
    //    private readonly ITimeSeries _timeseries;
    //    private readonly IMessageMapper _mapper;

    //    public OOBDefaultHandler(IStoreEvents store, ITimeSeries timeseries, IMessageMapper mapper)
    //    {
    //        _store = store;
    //       
    //        _timeseries = timeseries;
    //        _mapper = mapper;
    //    }

    //    public async Task Publish<T>(string Bucket, string StreamId, IEnumerable<IWritableEvent> Events, IDictionary<string, string> commitHeaders) where T : class, IEventSource
    //    {
    //        Logger.Debug(() => $"Publishing {Events.Count()} oob events");
    //        // Send metric OOB events out NSB, dont record them to the store
    //        if (typeof(T).GetInterfaces().Contains(typeof(Network.Stash.Entities.Metrics.IMetric)))
    //        {
    //            foreach (var header in commitHeaders)
    //            {
    //                if (header.Key == Headers.OriginatingHostId)
    //                {
    //                    //is added by bus in v5
    //                    continue;
    //                }

    //                _bus.OutgoingHeaders[header.Key] = header.Value != null ? header.Value.ToString() : null;
    //            }

    //            foreach (var @event in Events)
    //            {
    //                _bus.SetMessageHeader(@event.Event, "EventId", @event.EventId.ToString());
    //                _bus.SetMessageHeader(@event.Event, "EntityType", @event.Descriptor.EntityType);
    //                _bus.SetMessageHeader(@event.Event, "Timestamp", @event.Descriptor.Timestamp.ToString());
    //                _bus.SetMessageHeader(@event.Event, "Version", @event.Descriptor.Version.ToString());


    //                foreach (var header in @event.Descriptor.Headers)
    //                {
    //                    _bus.SetMessageHeader(@event.Event, header.Key, header.Value);
    //                }

    //                _bus.Publish(@event.Event);

    //            }

    //            return;
    //        }

    //        await _store.AppendEvents<T>(Bucket + ".OOB", StreamId, Events, commitHeaders);
    //    }

    //    public async Task<IEnumerable<IWritableEvent>> Retrieve<T>(string Bucket, string StreamId, Int32? Skip = null, Int32? Take = null, Boolean Ascending = true) where T : class, IEventSource
    //    {
    //        if (!Ascending)
    //            return await _store.GetEventsBackwards<T>(Bucket + ".OOB", StreamId, Skip, Take);
    //        return await _store.GetEvents<T>(Bucket + ".OOB", StreamId, Skip, Take);
    //    }
    //}
}
