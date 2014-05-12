using System.Collections.Generic;

namespace Demo.Library.Security
{
    /// <summary>
    /// Defines a <see cref="ITarget"/>
    /// </summary>
    public interface ITarget
    {
        /// <summary>
        /// Add a <see cref="ISecurable"/>
        /// </summary>
        /// <param name="securable"><see cref="ISecurityActor"/> to add</param>
        void AddSecurable(ISecurable securable);

        /// <summary>
        /// Get all <see cref="ISecurable">securables</see>
        /// </summary>
        IEnumerable<ISecurable> Securables { get; }

        /// <summary>
        /// Indicates whether this target can authorize the instance of this action
        /// </summary>
        /// <param name="action">An instance of the action to authorize</param>
        /// <returns>True if the <see cref="ISecurityTarget"/> can authorize this action, False otherwise</returns>
        bool CanAuthorize(object instance);

        /// <summary>
        /// Authorizes this <see cref="ISecurityTarget"/> for the instance of the action
        /// </summary>
        /// <param name="action">Instance of an action to be authorized</param>
        /// <returns>An <see cref="AuthorizeTargetResult"/> that indicates if the action was authorized or not</returns>
        AuthorizeTargetResult Authorize(object instance);

        /// <summary>
        /// Gets a description of the SecurityTarget.
        /// </summary>
        string Description { get; }
    }
}