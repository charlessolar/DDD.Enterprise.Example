using NLog.Targets;

namespace log4net.Appender
{
    [Target("Demo")]
    public class DemoAppender : TargetWithLayout
    {
        //private Queue<LoggingEvent> _events;
        //private DateTime _lastPost;
        //private String _url;
        //private String _stash;
        //private Guid _stashId;
        //private String _secret;
        //private static Boolean _initiated = false;

        //virtual public String ConnectionString { get; set; }
        //virtual public Int32 Interval { get; set; }
        //public DemoAppender()
        //{
        //    _events = new Queue<LoggingEvent>();
        //    _lastPost = DateTime.UtcNow;

        //    Interval = 30;

        //}
        //private void Init()
        //{
        //    var data = ConnectionString.Split(';');

        //    var url = data.FirstOrDefault(x => x.StartsWith("Url", StringComparison.CurrentCultureIgnoreCase));
        //    if (url == null)
        //        throw new ArgumentException("No URL parameter in pulse connection string");
        //    Guid stashId;
        //    var stashIdStr = data.FirstOrDefault(x => x.StartsWith("StashId=", StringComparison.CurrentCultureIgnoreCase));
        //    if (stashIdStr.IsNullOrEmpty() || !Guid.TryParse(stashIdStr.Substring(8), out stashId))
        //        throw new ArgumentException("No StashId parameter in pulse connection string");
        //    var stash = data.FirstOrDefault(x => x.StartsWith("Stash=", StringComparison.CurrentCultureIgnoreCase));
        //    if (stash.IsNullOrEmpty())
        //        throw new ArgumentException("No Stash parameter in pulse connection string");
        //    var secret = data.FirstOrDefault(x => x.StartsWith("Secret=", StringComparison.CurrentCultureIgnoreCase));
        //    if (secret.IsNullOrEmpty())
        //        throw new ArgumentException("No Secret parameter in pulse connection string");

        //    _url = url.Substring(4);
        //    _stash = stash.Substring(6);
        //    _stashId = stashId;
        //    _secret = secret.Substring(7);


        //    Identify();
        //}

        //public IEnumerable<LoggingEvent> GetEvents()
        //{
        //    lock (_events)
        //    {
        //        return _events.AsEnumerable();
        //    }
        //}
        //protected override void Append(LoggingEvent loggingEvent)
        //{
        //    if (!_initiated)
        //    {
        //        _initiated = true;
        //        ThreadPool.QueueUserWorkItem((_) =>
        //        {
        //            Init();
        //        });
        //    }

        //    loggingEvent.Fix = FixFlags.Partial;

        //    var count = 0;
        //    lock (_events)
        //    {
        //        _events.Enqueue(loggingEvent);
        //        count = _events.Count;
        //    }

        //    if ((loggingEvent.TimeStamp - _lastPost).TotalSeconds > Interval || count > 300 || loggingEvent.Level >= Level.Warn)
        //    {
        //        ThreadPool.QueueUserWorkItem((_) =>
        //        {
        //            PostEvents();
        //        });
        //    }
            
        //}

        //public void PostEvents()
        //{
        //    _lastPost = DateTime.UtcNow;

        //    var payload = new List<Dictionary<String, Object>>();

        //    lock (_events)
        //    {
        //        while (_events.Count != 0)
        //        {
        //            var loggingEvent = _events.Dequeue();

        //            var parameters = new Dictionary<String, Object>();

        //            parameters.Add("Level", loggingEvent.Level.ToString());
        //            parameters.Add("LoggerName", loggingEvent.LoggerName);
        //            parameters.Add("ThreadName", $"[{loggingEvent.ThreadName}]");
        //            parameters.Add("Domain", loggingEvent.Domain);
        //            parameters.Add("Machine", Environment.MachineName);
        //            parameters.Add("Message", loggingEvent.RenderedMessage);

        //            var isException = !String.IsNullOrEmpty(loggingEvent.GetExceptionString());
        //            parameters.Add("IsException", isException);
        //            if (isException)
        //                parameters.Add("Exception", loggingEvent.GetExceptionString());

        //            payload.Add(parameters);
        //        }
        //    }


        //    var client = new RestClient(_url);
        //    var request = new RestRequest("stash/{StashId}/bulk", Method.POST);

        //    request.AddUrlSegment("StashId", _stashId.ToString("N"));

        //    request.AddParameter("Secret", _secret);
        //    request.AddParameter("TickId", Guid.NewGuid());
        //    request.AddParameter("Event", "Log Event");
        //    request.AddParameter("Source", Environment.MachineName);
        //    request.AddParameter("Stamp", DateTime.UtcNow.ToISO8601());
            
        //    request.AddParameter("Data", JsonConvert.SerializeObject(payload));
        //    client.ExecuteAsync(request);
        //}
        
        //public void Identify()
        //{
        //    var client = new RestClient(_url);

        //    var init = new RestRequest("stash", Method.POST);
        //    init.AddParameter("StashId", _stashId);
        //    init.AddParameter("Secret", _secret);
        //    init.AddParameter("Name", _stash);
        //    init.AddParameter("Description", "Log4Net");

        //    var retries = 0;
        //    IRestResponse response;
        //    do
        //    {
        //        response = client.Execute(init);
        //        retries++;
        //        if (response.ResponseStatus == RestSharp.ResponseStatus.Completed)
        //            break;
        //    } while (retries < 5);
            
        //}

        //public void Clear()
        //{
        //    lock (_events)
        //    {
        //        _events.Clear();
        //    }
        //}
    }
}
