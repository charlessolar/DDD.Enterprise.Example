using NServiceBus;
using Demo.Library.Future;
using Demo.Library.Demo;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Domain.Infrastructure.Demo
{
    public class StatsMessageHandler : 
        IHandleMessages<ICommand>,
        IHandleMessages<IEvent>
    {
        private readonly IFuture _future;
        private readonly IDemo _pulse;
        private static bool _timed;
        private static int _commands;
        private static int _events;

        public StatsMessageHandler(IFuture future, IDemo pulse)
        {
            _future = future;
            _pulse = pulse;

            if (!_timed)
            {
                _future.FireRepeatedly(TimeSpan.FromSeconds(10), async () =>
                {
                    var commands = Interlocked.Exchange(ref _commands, 0);
                    var events = Interlocked.Exchange(ref _events, 0);
                    
                    await _pulse.Report("Commands", new { Count=commands}).ConfigureAwait(false);
                    await _pulse.Report("Application", new { Application="Domain", Events = events, Queries=0 }).ConfigureAwait(false);
                }, "Demo domain reporter");
                _timed = true;
            }
        }

        public Task Handle(ICommand e, IMessageHandlerContext ctx)
        {
            Interlocked.Add(ref _commands, 1);
            return Task.FromResult(0);
        }
        public Task Handle(IEvent e, IMessageHandlerContext ctx)
        {
            Interlocked.Add(ref _events, 1);
            return Task.FromResult(0);
        }
    }
}
