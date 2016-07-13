using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Future
{
    public interface IFuture
    {
        /// <summary>
        /// Schedules an action to be taken in the future
        /// </summary>
        /// <param name="At">Length of time to wait before firing the action</param>
        /// <param name="Action">The action to take in the future</param>
        void Fire(TimeSpan At, Action Action, String Description = "");
        void Fire(TimeSpan At, Func<Task> Action, String Description = "");
        /// <summary>
        /// Schedules an action to be taken in the future, with support for updating an action
        /// </summary>
        /// <param name="At">Length of time to wait before firing the action</param>
        /// <param name="Action">The action to take in the future</param>
        /// <param name="Id">A unique ID used to support updating an existing future event if it exists</param>
        void Fire(TimeSpan At, Action Action, String Id, String Description = "");
        void Fire(TimeSpan At, Func<Task> Action, String Id, String Description = "");
        void Fire(TimeSpan At, Object State, Action<Object> Action, String Description = "");
        void Fire(TimeSpan At, Object State, Func<Object, Task> Action, String Description = "");
        void Fire(TimeSpan At, Object State, Action<Object> Action, String Id, String Description = "");
        void Fire(TimeSpan At, Object State, Func<Object, Task> Action, String Id, String Description = "");
        /// <summary>
        /// Schedules an action to be taken in the future, with support for updating an action with support for varying timespans
        /// </summary>
        /// <param name="At">Length of time to wait before firing the action</param>
        /// <param name="Varies">A timespan of acceptable variance</param>
        /// <param name="Action">The action to take in the future</param>
        /// <param name="Id">A unique ID used to support updating an existing future event if it exists</param>
        void Fire(TimeSpan At, TimeSpan Varies, Action Action, String Id, String Description = "");
        void Fire(TimeSpan At, TimeSpan Varies, Func<Task> Action, String Id, String Description = "");
        void Fire(TimeSpan At, TimeSpan Varies, Action Action, String Description = "");
        void Fire(TimeSpan At, TimeSpan Varies, Func<Task> Action, String Description = "");

        void Fire(TimeSpan At, TimeSpan Varies, Object State, Action<Object> Action, String Description = "");
        void Fire(TimeSpan At, TimeSpan Varies, Object State, Func<Object, Task> Action, String Description = "");
        void Fire(TimeSpan At, TimeSpan Varies, Object State, Action<Object> Action, String Id, String Description = "");
        void Fire(TimeSpan At, TimeSpan Varies, Object State, Func<Object, Task> Action, String Id, String Description = "");

        void FireRepeatedly(TimeSpan Period, Action Action, String Description = "");
        void FireRepeatedly(TimeSpan Period, Func<Task> Action, String Description = "");
        void FireRepeatedly(TimeSpan Period, Action Action, String Id, String Description = "");
        void FireRepeatedly(TimeSpan Period, Func<Task> Action, String Id, String Description = "");

        Object RetreiveState(String Id);
        void CancelFire(String Id);
    }
}
