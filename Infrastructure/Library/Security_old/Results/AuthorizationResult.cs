using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Security
{
    /// <summary>
    /// Contains the result of an attempt to authorize an action.
    /// </summary>
    public class AuthorizationResult
    {
        private readonly List<AuthorizeDescriptorResult> _authorizationFailures = new List<AuthorizeDescriptorResult>();

        /// <summary>
        /// Gets any <see cref="AuthorizeDescriptorResult"> results</see> that were not authorized
        /// </summary>
        public IEnumerable<AuthorizeDescriptorResult> AuthorizationFailures
        {
            get { return _authorizationFailures.AsEnumerable(); }
        }

        /// <summary>
        /// Gets the result of the Authorization attempt for this action and <see cref="ISecurityDescriptor"/>
        /// </summary>
        public virtual bool IsAuthorized
        {
            get { return !_authorizationFailures.Any(); }
        }

        /// <summary>
        /// Processes instance of an <see cref="AuthorizeDescriptorResult"/>, adding failed authorizations to the AuthorizationFailures collection
        /// </summary>
        /// <param name="result">Result to process</param>
        public void ProcessAuthorizeDescriptorResult(AuthorizeDescriptorResult result)
        {
            if (!result.IsAuthorized)
                _authorizationFailures.Add(result);
        }

        /// <summary>
        /// Gets all the broken <see cref="ISecurityRule">rules</see> for this authorization attempt
        /// </summary>
        /// <returns>A string describing each broken rule or an empty enumerable if there are none</returns>
        public virtual IEnumerable<string> BuildFailedAuthorizationMessages()
        {
            var messages = new List<string>();
            foreach (var result in AuthorizationFailures)
            {
                messages.AddRange(result.BuildFailedAuthorizationMessages());
            }
            return messages;
        }
    }
}