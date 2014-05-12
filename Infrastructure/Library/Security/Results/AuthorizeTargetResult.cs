using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Security
{
    /// <summary>
    /// Represents the result of an authorization of a <see cref="ISecurityTarget"/>
    /// </summary>
    public class AuthorizeTargetResult
    {
        private readonly List<AuthorizeSecurableResult> _authorizationFailures = new List<AuthorizeSecurableResult>();

        /// <summary>
        /// Instantiates an instance of <see cref="AuthorizeTargetResult"/> for the specificed <see cref="ISecurityTarget"/>
        /// </summary>
        /// <param name="target"><see cref="ISecurityTarget"/> that this <see cref="AuthorizeTargetResult"/> pertains to.</param>
        public AuthorizeTargetResult(ITarget target)
        {
            Target = target;
        }

        /// <summary>
        /// Gets the <see cref="ISecurityTarget"/> that this <see cref="AuthorizeTargetResult"/> pertains to.
        /// </summary>
        public ITarget Target { get; private set; }

        /// <summary>
        /// Gets the <see cref="AuthorizeSecurableResult"/> for each failed <see cref="ISecurable"/> on the <see cref="ISecurityTarget"/>
        /// </summary>
        public IEnumerable<AuthorizeSecurableResult> AuthorizationFailures
        {
            get { return _authorizationFailures.AsEnumerable(); }
        }

        /// <summary>
        /// Indicates if the action instance has been authorized by the <see cref="ISecurityTarget"/>
        /// </summary>
        public virtual bool IsAuthorized
        {
            get { return !AuthorizationFailures.Any(); }
        }

        /// <summary>
        /// Processes an <see cref="AuthorizeSecurableResult"/>, adding it to the collection of AuthorizationFailures if appropriate
        /// </summary>
        /// <param name="result">An <see cref="AuthorizeSecurableResult"/> for a <see cref="ISecurable"/></param>
        public void ProcessAuthorizeSecurableResult(AuthorizeSecurableResult result)
        {
            if (!result.IsAuthorized)
                _authorizationFailures.Add(result);
        }

        /// <summary>
        /// Builds a collection of strings that show Target/Securable for each broken or erroring rule in<see cref="AuthorizeTargetResult"/>
        /// </summary>
        /// <returns>A collection of strings</returns>
        public virtual IEnumerable<string> BuildFailedAuthorizationMessages()
        {
            return from result in AuthorizationFailures from message in result.BuildFailedAuthorizationMessages() select Target.Description + "/" + message;
        }
    }
}