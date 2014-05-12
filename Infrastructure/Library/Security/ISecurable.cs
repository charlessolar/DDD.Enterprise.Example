using System;
using System.Collections.Generic;

namespace Demo.Library.Security
{
    /// <summary>
    /// Defines something that can be secured
    /// </summary>
    public interface ISecurable
    {
        void AddActor(IActor actor);

        IEnumerable<IActor> Actors { get; }

        bool CanAuthorize(object instance);

        AuthorizeSecurableResult Authorize(object instance);

        string Description { get; }
    }
}