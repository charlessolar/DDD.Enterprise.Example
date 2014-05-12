using System.Collections.Generic;

namespace Demo.Library.Security
{
    /// <summary>
    /// Defines an action that is subject to security
    /// </summary>
    public interface IAction
    {
        void AddTarget(ITarget target);

        IEnumerable<ITarget> Targets { get; }

        /// <summary>
        /// Checks if any targets can authorize the action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        bool CanAuthorize(object instance);

        AuthorizeActionResult Authorize(object instance);

        string Description { get; }
    }
}