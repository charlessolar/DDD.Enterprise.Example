using System.Collections.Generic;
using System.Linq;

namespace Demo.Library.Security.Securables
{
    /// <summary>
    /// Represents a base implementation of<see cref="ISecurable"/>
    /// </summary>
    public class Securable : ISecurable
    {
        private readonly List<IActor> _actors = new List<IActor>();

        /// <summary>
        /// Instantiates an instance of <see cref="Securable"/>
        /// </summary>
        /// <param name="securableDescription">Description of the Securable</param>
        public Securable(string securableDescription)
        {
            Description = securableDescription ?? string.Empty;
        }

#pragma warning disable 1591 // Xml Comments

        public virtual void AddActor(IActor actor)
        {
            _actors.Add(actor);
        }

        public virtual IEnumerable<IActor> Actors { get { return _actors; } }

        public virtual bool CanAuthorize(object instance)
        {
            return false;
        }

        public virtual AuthorizeSecurableResult Authorize(object instance)
        {
            var result = new AuthorizeSecurableResult(this);
            foreach (var actor in _actors)
            {
                result.ProcessAuthorizeActorResult(actor.IsAuthorized(instance));
            }
            return result;
        }

        public string Description { get; private set; }
#pragma warning restore 1591 // Xml Comments
    }
}