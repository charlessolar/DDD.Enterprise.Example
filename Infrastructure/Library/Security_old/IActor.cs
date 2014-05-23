using System;
using System.Collections.Generic;

namespace Demo.Library.Security
{
    public interface IActor
    {
        void AddRule(IRule rule);
        IEnumerable<IRule> Rules { get; }

        AuthorizeActorResult IsAuthorized(object instance);

        string Description { get; }
    }
}