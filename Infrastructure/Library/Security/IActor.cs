using System;
using System.Collections.Generic;

namespace Demo.Library.Security
{
    public interface IActor
    {
        AuthorizeActorResult IsAuthorized(object instance);

        string Description { get; }
    }
}