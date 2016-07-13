
using NLog;
using ServiceStack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Library.Future
{
    public class Future : IFuture
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private class Timed : IDisposable
        {
            public String Id { get; set; }
            public String Description { get; set; }
            public DateTime Expires { get; set; }
            public Func<Object, Task> Action { get; set; }
            public Timer Timer { get; set; }
            public Object State { get; set; }
            public Object Lock { get; set; }

            public void Dispose()
            {
                this.Timer.Dispose();
            }
        }
        private readonly ConcurrentDictionary<String, Timed> _timers;

        public Future()
        {
            _timers = new ConcurrentDictionary<String, Timed>();
        }

        public void Fire(TimeSpan At, Action Action, String Description = "")
        {
            var id = Guid.NewGuid().ToString();
            Fire(At, null, (_) => { Action(); }, id, Description);
        }
        public void Fire(TimeSpan At, Func<Task> Action, String Description = "")
        {
            var id = Guid.NewGuid().ToString();
            Fire(At, null, (_) => Action(), id, Description);
        }
        public void Fire(TimeSpan At, Action Action, String Id, String Description = "")
        {
            Fire(At, null, (_) => { Action(); }, Id, Description);
        }
        public void Fire(TimeSpan At, Func<Task> Action, String Id, String Description = "")
        {
            Fire(At, null, (_) => Action(), Id, Description);
        }
        public void Fire(TimeSpan At, Object State, Action<Object> Action, String Description = "")
        {
            Fire(At, State, Action, Guid.NewGuid().ToString(), Description);
        }
        public void Fire(TimeSpan At, Object State, Func<Object, Task> Action, String Description = "")
        {
            Fire(At, State, Action, Guid.NewGuid().ToString(), Description);
        }
        public void Fire(TimeSpan At, Object State, Action<Object> Action, String Id, String Description = "")
        {
            Fire(At, State, (_) =>
            {
                Action(_);
                return Task.FromResult(0);
            }, Id, Description);
        }
        public void Fire(TimeSpan At, Object State, Func<Object, Task> Action, String Id, String Description = "")
        {
            State = State ?? new object();

            _timers.AddOrUpdate(Id, _ =>
            {

                Logger.Debug("Registering new timer with id {0} to run in {1} seconds. Description: {2}", Id, At.TotalSeconds, Description);
                var timer = new Timed
                {
                    Id = Id,
                    Description = Description,
                    Expires = DateTime.UtcNow.Add(At),
                    State = State,
                    Action = Action,
                    Lock = new object()
                };

                timer.Timer = new Timer((state) =>
                {
                    var t = (Timed)state;

                    Logger.Debug("Timer id {0} firing", t.Id);
                    Timed placeholder;
                    _timers.TryRemove(t.Id, out placeholder);

                    if (!Monitor.TryEnter(t.Lock, TimeSpan.FromSeconds(10)))
                    {
                        Logger.Warn("Failed to run timer id {0} Description: {1} - could not aquire lock on state", t.Id, t.Description);
                        return;
                    }


                    try
                    {
                        Task.Run(async () =>
                        {
                            await t.Action(t.State);
                        }).Wait();
                    }
                    catch (AggregateException e)
                    {
                        Logger.Error("Timer id {0} threw an exception\nDescription: {1}\nException: {2}", t.Id, t.Description, e);
                    }

                    Monitor.Exit(t.Lock);
                }, timer, Convert.ToInt64(At.TotalMilliseconds), Timeout.Infinite);
                return timer;
            }, (_, timer) =>
            {
                Logger.Debug("Updating existing timer with id {0} to run in {1} seconds. Description: {2}", Id, At.TotalSeconds, Description);
                timer.Action = Action;
                timer.State = State;
                timer.Description = Description;

                return timer;
            });

        }


        public void Fire(TimeSpan At, TimeSpan Varies, Action Action, String Description = "")
        {
            Fire(At, Varies, null, (_) => { Action(); }, Guid.NewGuid().ToString(), Description);
        }
        public void Fire(TimeSpan At, TimeSpan Varies, Func<Task> Action, String Description = "")
        {
            Fire(At, Varies, null, (_) => Action(), Guid.NewGuid().ToString(), Description);
        }
        public void Fire(TimeSpan At, TimeSpan Varies, Action Action, String Id, String Description = "")
        {
            Fire(At, Varies, null, (_) => { Action(); }, Id, Description);
        }
        public void Fire(TimeSpan At, TimeSpan Varies, Func<Task> Action, String Id, String Description = "")
        {
            Fire(At, Varies, null, (_) => Action(), Id, Description);
        }
        public void Fire(TimeSpan At, TimeSpan Varies, Object State, Action<Object> Action, String Description = "") {
            Fire(At, Varies, State, Action, Guid.NewGuid().ToString(), Description);
        }
        public void Fire(TimeSpan At, TimeSpan Varies, Object State, Func<Object, Task> Action, String Description = "")
        {
            Fire(At, Varies, State, Action, Guid.NewGuid().ToString(), Description);
        }
        public void Fire(TimeSpan At, TimeSpan Varies, Object State, Action<Object> Action, String Id, String Description = "")
        {
            Fire(At, Varies, State, (_) =>
            {
                Action(_);
                return Task.FromResult(0);
            }, Id, Description);
        }
        public void Fire(TimeSpan At, TimeSpan Varies, Object State, Func<Object, Task> Action, String Id, String Description = "")
        {
            var random = new Random();
            var toAdd = random.Next((Int32)Math.Floor(Varies.TotalSeconds * 0.2), (Int32)Varies.TotalSeconds);

            Fire(At.Add(TimeSpan.FromSeconds(toAdd)), State, Action, Id, Description);
        }




        public void FireRepeatedly(TimeSpan Period, Action Action, String Description = "")
        {
            FireRepeatedly(Period, null, (_) => { Action(); }, Guid.NewGuid().ToString(), Description);
        }
        public void FireRepeatedly(TimeSpan Period, Func<Task> Action, String Description = "")
        {
            FireRepeatedly(Period, null, (_) => Action(), Guid.NewGuid().ToString(), Description);
        }

        public void FireRepeatedly(TimeSpan Period, Action Action, String Id, String Description = "")
        {
            FireRepeatedly(Period, null, (_) => { Action(); }, Id, Description);
        }
        public void FireRepeatedly(TimeSpan Period, Func<Task> Action, String Id, String Description = "")
        {
            FireRepeatedly(Period, null, (_) => Action(), Id, Description);
        }
        public void FireRepeatedly(TimeSpan Period, Object State, Action<Object> Action, String Description = "")
        {
            FireRepeatedly(Period, State, Action, Guid.NewGuid().ToString(), Description);
        }
        public void FireRepeatedly(TimeSpan Period, Object State, Func<Object, Task> Action, String Description = "")
        {
            FireRepeatedly(Period, State, Action, Guid.NewGuid().ToString(), Description);
        }
        public void FireRepeatedly(TimeSpan Period, Object State, Action<Object> Action, String Id, String Description = "")
        {
            FireRepeatedly(Period, State, (_) =>
            {
                Action(_);
                return Task.FromResult(0);
            }, Id, Description);
        }
        public void FireRepeatedly(TimeSpan Period, Object State, Func<Object, Task> Action, String Id, String Description = "")
        {
            State = State ?? new object();

            _timers.AddOrUpdate(Id, _ =>
            {
                Logger.Debug("Registering new periodic timer with id {0} to run every {1} seconds. Description: {2}", Id, Period.TotalSeconds, Description);
                var timer = new Timed
                {
                    Id = Id,
                    Description = Description,
                    Expires = DateTime.UtcNow,
                    State = State,
                    Action = Action,
                    Lock = new object()
                };

                timer.Timer = new Timer((state) =>
                {
                    var t = (Timed)state;
                    Logger.Debug("Periodic timer id {0} firing. Description: {1}", t.Id, t.Description);

                    if (!Monitor.TryEnter(t.Lock, Period))
                    {
                        Logger.Warn("Failed to run periodic timer id {0} Description: {1} - could not aquire lock on state.  This could be due to the timer action taking too long", t.Id, t.Description);
                        return;
                    }
                    try
                    {
                        Task.Run(async () =>
                        {
                            await t.Action(t.State);
                        }).Wait();
                    }
                    catch (AggregateException e)
                    {
                        Logger.Error("Periodic timer id {0} threw an exception\nDescription: {1}\nException: {2}", t.Id, t.Description, e);
                    }

                    Monitor.Exit(t.Lock);
                }, timer, 0, Convert.ToInt64(Period.TotalMilliseconds));
                return timer;
            }, (_, timer) =>
            {
                Logger.Debug("Updating existing periodic timer with id {0} to run every {1} seconds. Description: {2}", Id, Period.TotalSeconds, Description);
                timer.Action = Action;

                // Lock the state incase the timer is currently executing, we shouldn't change the state mid-timer fire
                lock (timer.State)
                {
                    timer.State = State;
                }

                timer.Description = Description;

                return timer;
            });

        }
        public void CancelFire(String Id)
        {
            Timed timer;
            _timers.TryRemove(Id, out timer);
            timer.Dispose();
        }
        public Object RetreiveState(String Id)
        {
            Timed timed;
            if (!_timers.TryGetValue(Id, out timed))
                return null;
            return timed.State;
        }
    }
}
