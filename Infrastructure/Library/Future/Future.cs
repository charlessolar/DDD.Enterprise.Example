
using NLog;
using StructureMap;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Demo.Library.Future
{
    public class Future : IFuture
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IContainer _container;

        private class Timed : IDisposable
        {
            public string Id { get; set; }
            public string Description { get; set; }
            public DateTime Expires { get; set; }
            public Func<object, Task> Action { get; set; }
            public Timer Timer { get; set; }
            public object State { get; set; }
            public object Lock { get; set; }

            public void Dispose()
            {
                this.Timer.Dispose();
            }
        }
        private readonly ConcurrentDictionary<string, Timed> _timers;

        public Future(IContainer container)
        {
            _container = container;
            _timers = new ConcurrentDictionary<string, Timed>();
        }

        public void Fire(TimeSpan at, Action action, string description = "")
        {
            var id = Guid.NewGuid().ToString();
            Fire(at, null, (_) => { action(); }, id, description);
        }
        public void Fire(TimeSpan at, Func<Task> action, string description = "")
        {
            var id = Guid.NewGuid().ToString();
            Fire(at, null, (_) => action(), id, description);
        }
        public void Fire(TimeSpan at, Action action, string id, string description = "")
        {
            Fire(at, null, (_) => { action(); }, id, description);
        }
        public void Fire(TimeSpan at, Func<Task> action, string id, string description = "")
        {
            Fire(at, null, (_) => action(), id, description);
        }
        public void Fire(TimeSpan at, object state, Action<object> action, string description = "")
        {
            Fire(at, state, action, Guid.NewGuid().ToString(), description);
        }
        public void Fire(TimeSpan at, object state, Func<object, Task> action, string description = "")
        {
            Fire(at, state, action, Guid.NewGuid().ToString(), description);
        }
        public void Fire(TimeSpan at, object state, Action<object> action, string id, string description = "")
        {
            Fire(at, state, (_) =>
            {
                action(_);
                return Task.FromResult(0);
            }, id, description);
        }
        public void Fire(TimeSpan at, object State, Func<object, Task> action, string id, string description = "")
        {
            State = State ?? new object();

            _timers.AddOrUpdate(id, _ =>
            {

                Logger.Debug("Registering new timer with id {0} to run in {1} seconds. Description: {2}", id, at.TotalSeconds, description);
                var timer = new Timed
                {
                    Id = id,
                    Description = description,
                    Expires = DateTime.UtcNow.Add(at),
                    State = State,
                    Action = action,
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
                            await t.Action(t.State).ConfigureAwait(false);
                        }).Wait();
                    }
                    catch (AggregateException e)
                    {
                        Logger.Error("Timer id {0} threw an exception\nDescription: {1}\nException: {2}", t.Id, t.Description, e);
                    }
                    finally
                    {
                        Monitor.Exit(t.Lock);
                    }

                }, timer, Convert.ToInt64(at.TotalMilliseconds), Timeout.Infinite);
                return timer;
            }, (_, timer) =>
            {
                Logger.Debug("Updating existing timer with id {0} to run in {1} seconds. Description: {2}", id, at.TotalSeconds, description);
                timer.Action = action;
                timer.State = State;
                timer.Description = description;

                return timer;
            });

        }

        public void Fire<TFirst>(TimeSpan at, Action<TFirst> action, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                action(first);
            }, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, Action<TFirst, TSecond> action, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(first, second);
            }, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, Action<TFirst> action, string id, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                action(first);
            }, id, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, Action<TFirst, TSecond> action, string id, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(first, second);
            }, id, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, Func<TFirst, Task> action, string id, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(first);
            }, id, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, Func<TFirst, TSecond, Task> action, string id, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(first, second);
            }, id, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, object State, Action<object, TFirst> action, string id, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                action(state, first);
            }, id, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, object State, Action<object, TFirst, TSecond> action, string id, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(state, first, second);
            }, id, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, object State, Func<object, TFirst, Task> action, string id, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(state, first);
            }, id, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, object State, Func<object, TFirst, TSecond, Task> action, string id, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(state, first, second);
            }, id, description: description);
        }


        public void Fire(TimeSpan at, TimeSpan varies, Action action, string description = "")
        {
            Fire(at, varies, null, (_) => { action(); }, Guid.NewGuid().ToString(), description);
        }
        public void Fire(TimeSpan at, TimeSpan varies, Func<Task> action, string description = "")
        {
            Fire(at, varies, null, (_) => action(), Guid.NewGuid().ToString(), description);
        }
        public void Fire(TimeSpan at, TimeSpan varies, Action action, string id, string description = "")
        {
            Fire(at, varies, null, (_) => { action(); }, id, description);
        }
        public void Fire(TimeSpan at, TimeSpan varies, Func<Task> action, string id, string description = "")
        {
            Fire(at, varies, null, (_) => action(), id, description);
        }
        public void Fire(TimeSpan at, TimeSpan varies, object state, Action<object> action, string description = "")
        {
            Fire(at, varies, state, action, Guid.NewGuid().ToString(), description);
        }
        public void Fire(TimeSpan at, TimeSpan varies, object state, Func<object, Task> action, string description = "")
        {
            Fire(at, varies, state, action, Guid.NewGuid().ToString(), description);
        }
        public void Fire(TimeSpan at, TimeSpan varies, object state, Action<object> action, string id, string description = "")
        {
            Fire(at, varies, state, (_) =>
            {
                action(_);
                return Task.FromResult(0);
            }, id, description);
        }
        public void Fire(TimeSpan at, TimeSpan varies, object state, Func<object, Task> action, string id, string description = "")
        {
            var random = new Random();
            var toAdd = random.Next((int)Math.Floor(varies.TotalSeconds * 0.2), (int)varies.TotalSeconds);

            Fire(at.Add(TimeSpan.FromSeconds(toAdd)), state, action, id, description);
        }


        public void Fire<TFirst>(TimeSpan at, TimeSpan varies, Action<TFirst> action, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                action(first);
            }, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, Action<TFirst, TSecond> action, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(first, second);
            }, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, TimeSpan varies, Action<TFirst> action, string id, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                action(first);
            }, id, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, Action<TFirst, TSecond> action, string id, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(first, second);
            }, id, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, TimeSpan varies, Func<TFirst, Task> action, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(first);
            }, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, Func<TFirst, TSecond, Task> action, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(first, second);
            }, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, TimeSpan varies, Func<TFirst, Task> action, string id, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(first);
            }, id, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, Func<TFirst, TSecond, Task> action, string id, string description = "")
        {
            Fire(at, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(first, second);
            }, id, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, TimeSpan varies, object State, Action<object, TFirst> action, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                action(state, first);
            }, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, object State, Action<object, TFirst, TSecond> action, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(state, first, second);
            }, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, TimeSpan varies, object State, Action<object, TFirst> action, string id, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                action(state, first);
            }, id, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, object State, Action<object, TFirst, TSecond> action, string id, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(state, first, second);
            }, id, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, TimeSpan varies, object State, Func<object, TFirst, Task> action, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(state, first);
            }, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, object State, Func<object, TFirst, TSecond, Task> action, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(state, first, second);
            }, description: description);
        }
        public void Fire<TFirst>(TimeSpan at, TimeSpan varies, object State, Func<object, TFirst, Task> action, string id, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(state, first);
            }, id, description: description);
        }
        public void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, object State, Func<object, TFirst, TSecond, Task> action, string id, string description = "")
        {
            Fire(at, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(state, first, second);
            }, id, description: description);
        }


        public void FireRepeatedly(TimeSpan period, Action action, string description = "")
        {
            FireRepeatedly(period, null, (_) => { action(); }, Guid.NewGuid().ToString(), description);
        }
        public void FireRepeatedly(TimeSpan period, Func<Task> action, string description = "")
        {
            FireRepeatedly(period, null, (_) => action(), Guid.NewGuid().ToString(), description);
        }

        public void FireRepeatedly(TimeSpan period, Action action, string id, string description = "")
        {
            FireRepeatedly(period, null, (_) => { action(); }, id, description);
        }
        public void FireRepeatedly(TimeSpan period, Func<Task> action, string id, string description = "")
        {
            FireRepeatedly(period, null, (_) => action(), id, description);
        }
        public void FireRepeatedly(TimeSpan period, object state, Action<object> action, string description = "")
        {
            FireRepeatedly(period, state, action, Guid.NewGuid().ToString(), description);
        }
        public void FireRepeatedly(TimeSpan period, object state, Func<object, Task> action, string description = "")
        {
            FireRepeatedly(period, state, action, Guid.NewGuid().ToString(), description);
        }
        public void FireRepeatedly(TimeSpan period, object state, Action<object> action, string id, string description = "")
        {
            FireRepeatedly(period, state, (_) =>
            {
                action(_);
                return Task.FromResult(0);
            }, id, description);
        }
        public void FireRepeatedly(TimeSpan period, object State, Func<object, Task> action, string id, string description = "")
        {
            State = State ?? new object();

            _timers.AddOrUpdate(id, _ =>
            {
                Logger.Debug("Registering new periodic timer with id {0} to run every {1} seconds. Description: {2}", id, period.TotalSeconds, description);
                var timer = new Timed
                {
                    Id = id,
                    Description = description,
                    Expires = DateTime.UtcNow,
                    State = State,
                    Action = action,
                    Lock = new object()
                };

                timer.Timer = new Timer((state) =>
                {
                    var t = (Timed)state;
                    Logger.Debug("Periodic timer id {0} firing. Description: {1}", t.Id, t.Description);

                    if (!Monitor.TryEnter(t.Lock, period))
                    {
                        Logger.Warn("Failed to run periodic timer id {0} Description: {1} - could not aquire lock on state.  This could be due to the timer action taking too long", t.Id, t.Description);
                        return;
                    }
                    try
                    {
                        Task.Run(async () =>
                        {
                            await t.Action(t.State).ConfigureAwait(false);
                        }).Wait();
                    }
                    catch (AggregateException e)
                    {
                        Logger.Error("Periodic timer id {0} threw an exception\nDescription: {1}\nException: {2}", t.Id, t.Description, e);
                    }
                    finally
                    {
                        Monitor.Exit(t.Lock);
                    }

                }, timer, 0, Convert.ToInt64(period.TotalMilliseconds));
                return timer;
            }, (_, timer) =>
            {
                Logger.Debug("Updating existing periodic timer with id {0} to run every {1} seconds. Description: {2}", id, period.TotalSeconds, description);
                timer.Action = action;

                // Lock the state incase the timer is currently executing, we shouldn't change the state mid-timer fire
                lock (timer.State)
                {
                    timer.State = State;
                }

                timer.Description = description;

                return timer;
            });

        }



        public void FireRepeatedly<TFirst>(TimeSpan period, Action<TFirst> action, string description = "")
        {
            FireRepeatedly(period, () =>
            {
                var first = _container.GetInstance<TFirst>();
                action(first);
            }, description: description);
        }
        public void FireRepeatedly<TFirst, TSecond>(TimeSpan period, Action<TFirst, TSecond> action, string description = "")
        {
            FireRepeatedly(period, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(first, second);
            }, description: description);
        }
        public void FireRepeatedly<TFirst>(TimeSpan period, Func<TFirst, Task> action, string description = "")
        {
            FireRepeatedly(period, () =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(first);
            }, description: description);
        }
        public void FireRepeatedly<TFirst, TSecond>(TimeSpan period, Func<TFirst, TSecond, Task> action, string description = "")
        {
            FireRepeatedly(period, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(first, second);
            }, description: description);
        }
        public void FireRepeatedly<TFirst>(TimeSpan period, Action<TFirst> action, string id, string description = "")
        {
            FireRepeatedly(period, () =>
            {
                var first = _container.GetInstance<TFirst>();
                action(first);
            }, id, description: description);
        }
        public void FireRepeatedly<TFirst, TSecond>(TimeSpan period, Action<TFirst, TSecond> action, string id, string description = "")
        {
            FireRepeatedly(period, () =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(first, second);
            }, id, description: description);
        }
        public void FireRepeatedly<TFirst>(TimeSpan period, object State, Action<object, TFirst> action, string id, string description = "")
        {
            FireRepeatedly(period, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                action(state, first);
            }, id, description: description);
        }
        public void FireRepeatedly<TFirst, TSecond>(TimeSpan period, object State, Action<object, TFirst, TSecond> action, string id, string description = "")
        {
            FireRepeatedly(period, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                action(state, first, second);
            }, id, description: description);
        }
        public void FireRepeatedly<TFirst>(TimeSpan period, object State, Func<object, TFirst, Task> action, string id, string description = "")
        {
            FireRepeatedly(period, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(state, first);
            }, id, description: description);
        }
        public void FireRepeatedly<TFirst, TSecond>(TimeSpan period, object State, Func<object, TFirst, TSecond, Task> action, string id, string description = "")
        {
            FireRepeatedly(period, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(state, first, second);
            }, id, description: description);
        }
        public void FireRepeatedly<TFirst>(TimeSpan period, object State, Func<object, TFirst, Task> action, string description = "")
        {
            FireRepeatedly(period, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                return action(state, first);
            }, description: description);
        }
        public void FireRepeatedly<TFirst, TSecond>(TimeSpan period, object State, Func<object, TFirst, TSecond, Task> action, string description = "")
        {
            FireRepeatedly(period, State, (state) =>
            {
                var first = _container.GetInstance<TFirst>();
                var second = _container.GetInstance<TSecond>();
                return action(state, first, second);
            }, description: description);
        }



        public void CancelFire(string id)
        {
            Timed timer;
            if(_timers.TryRemove(id, out timer))
                timer.Dispose();
        }
        public object RetreiveState(string id)
        {
            Timed timed;
            if (!_timers.TryGetValue(id, out timed))
                return null;
            return timed.State;
        }
    }
}
