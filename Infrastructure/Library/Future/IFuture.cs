using System;
using System.Threading.Tasks;

namespace Demo.Library.Future
{
    public interface IFuture
    {
        /// <summary>
        /// Schedules an action to be taken in the future
        /// </summary>
        /// <param name="at">Length of time to wait before firing the action</param>
        /// <param name="action">The action to take in the future</param>
        void Fire(TimeSpan at, Action action, string description = "");
        void Fire(TimeSpan at, Func<Task> action, string description = "");
        /// <summary>
        /// Schedules an action to be taken in the future, with support for updating an action
        /// </summary>
        /// <param name="at">Length of time to wait before firing the action</param>
        /// <param name="action">The action to take in the future</param>
        /// <param name="id">A unique ID used to support updating an existing future event if it exists</param>
        void Fire(TimeSpan at, Action action, string id, string description = "");
        void Fire(TimeSpan at, Func<Task> action, string id, string description = "");
        void Fire(TimeSpan at, object state, Action<object> action, string description = "");
        void Fire(TimeSpan at, object state, Func<object, Task> action, string description = "");
        void Fire(TimeSpan at, object state, Action<object> action, string id, string description = "");
        void Fire(TimeSpan at, object state, Func<object, Task> action, string id, string description = "");

        /// <summary>
        /// Like fire, except will attempt to build a dependency and send it to the action
        /// </summary>
        /// <typeparam name="TFirst"></typeparam>
        /// <param name="at"></param>
        /// <param name="Varies"></param>
        /// <param name="action"></param>
        /// <param name="description"></param>
        void Fire<TFirst>(TimeSpan at, Action<TFirst> action, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, Action<TFirst, TSecond> action, string description = "");
        void Fire<TFirst>(TimeSpan at, Action<TFirst> action, string id, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, Action<TFirst, TSecond> action, string id, string description = "");
        void Fire<TFirst>(TimeSpan at, Func<TFirst, Task> action, string id, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, Func<TFirst, TSecond, Task> action, string id, string description = "");
        void Fire<TFirst>(TimeSpan at, object state, Action<object, TFirst> action, string id, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, object state, Action<object, TFirst, TSecond> action, string id, string description = "");
        void Fire<TFirst>(TimeSpan at, object state, Func<object, TFirst, Task> action, string id, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, object state, Func<object, TFirst, TSecond, Task> action, string id, string description = "");

        /// <summary>
        /// Schedules an action to be taken in the future, with support for updating an action with support for varying timespans
        /// </summary>
        /// <param name="at">Length of time to wait before firing the action</param>
        /// <param name="varies">A timespan of acceptable variance</param>
        /// <param name="action">The action to take in the future</param>
        /// <param name="id">A unique ID used to support updating an existing future event if it exists</param>
        void Fire(TimeSpan at, TimeSpan varies, Action action, string id, string description = "");
        void Fire(TimeSpan at, TimeSpan varies, Func<Task> action, string id, string description = "");
        void Fire(TimeSpan at, TimeSpan varies, Action action, string description = "");
        void Fire(TimeSpan at, TimeSpan varies, Func<Task> action, string description = "");
        void Fire(TimeSpan at, TimeSpan varies, object state, Action<object> action, string description = "");
        void Fire(TimeSpan at, TimeSpan varies, object state, Func<object, Task> action, string description = "");
        void Fire(TimeSpan at, TimeSpan varies, object state, Action<object> action, string id, string description = "");
        void Fire(TimeSpan at, TimeSpan varies, object state, Func<object, Task> action, string id, string description = "");


        void Fire<TFirst>(TimeSpan at, TimeSpan varies, Action<TFirst> action, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, Action<TFirst, TSecond> action, string description = "");
        void Fire<TFirst>(TimeSpan at, TimeSpan varies, Action<TFirst> action, string id, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, Action<TFirst, TSecond> action, string id, string description = "");
        void Fire<TFirst>(TimeSpan at, TimeSpan varies, Func<TFirst, Task> action, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, Func<TFirst, TSecond, Task> action, string description = "");
        void Fire<TFirst>(TimeSpan at, TimeSpan varies, Func<TFirst, Task> action, string id, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, Func<TFirst, TSecond, Task> action, string id, string description = "");
        void Fire<TFirst>(TimeSpan at, TimeSpan varies, object state, Action<object, TFirst> action, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, object state, Action<object, TFirst, TSecond> action, string description = "");
        void Fire<TFirst>(TimeSpan at, TimeSpan varies, object state, Action<object, TFirst> action, string id, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, object state, Action<object, TFirst, TSecond> action, string id, string description = "");
        void Fire<TFirst>(TimeSpan at, TimeSpan varies, object state, Func<object, TFirst, Task> action, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, object state, Func<object, TFirst, TSecond, Task> action, string description = "");
        void Fire<TFirst>(TimeSpan at, TimeSpan varies, object state, Func<object, TFirst, Task> action, string id, string description = "");
        void Fire<TFirst, TSecond>(TimeSpan at, TimeSpan varies, object state, Func<object, TFirst, TSecond, Task> action, string id, string description = "");


        void FireRepeatedly(TimeSpan period, Action action, string description = "");
        void FireRepeatedly(TimeSpan period, Func<Task> action, string description = "");
        void FireRepeatedly(TimeSpan period, Action action, string id, string description = "");
        void FireRepeatedly(TimeSpan period, Func<Task> action, string id, string description = "");
        void FireRepeatedly(TimeSpan period, object state, Func<object, Task> action, string id, string description = "");
        void FireRepeatedly(TimeSpan period, object state, Action<object> action, string id, string description = "");
        void FireRepeatedly(TimeSpan period, object state, Func<object, Task> action, string description = "");
        void FireRepeatedly(TimeSpan period, object state, Action<object> action, string description = "");

        void FireRepeatedly<TFirst>(TimeSpan period, Action<TFirst> action, string description = "");
        void FireRepeatedly<TFirst, TSecond>(TimeSpan period, Action<TFirst, TSecond> action, string description = "");
        void FireRepeatedly<TFirst>(TimeSpan period, Func<TFirst, Task> action, string description = "");
        void FireRepeatedly<TFirst, TSecond>(TimeSpan period, Func<TFirst, TSecond, Task> action, string description = "");
        void FireRepeatedly<TFirst>(TimeSpan period, Action<TFirst> action, string id, string description = "");
        void FireRepeatedly<TFirst, TSecond>(TimeSpan period, Action<TFirst, TSecond> action, string id, string description = "");
        void FireRepeatedly<TFirst>(TimeSpan period, object state, Action<object, TFirst> action, string id, string description = "");
        void FireRepeatedly<TFirst, TSecond>(TimeSpan period, object state, Action<object, TFirst, TSecond> action, string id, string description = "");
        void FireRepeatedly<TFirst>(TimeSpan period, object state, Func<object, TFirst, Task> action, string id, string description = "");
        void FireRepeatedly<TFirst, TSecond>(TimeSpan period, object state, Func<object, TFirst, TSecond, Task> action, string id, string description = "");
        void FireRepeatedly<TFirst>(TimeSpan period, object state, Func<object, TFirst, Task> action, string description = "");
        void FireRepeatedly<TFirst, TSecond>(TimeSpan period, object state, Func<object, TFirst, TSecond, Task> action, string description = "");


        object RetreiveState(string id);
        void CancelFire(string id);
    }
}
