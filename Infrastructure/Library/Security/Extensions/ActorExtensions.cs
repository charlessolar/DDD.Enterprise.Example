using Demo.Library.Security.Actors;
using Demo.Library.Security.Securables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public static class ActorExtensions
    {
        public static UserActor Users(this ISecurable securable)
        {
            var actor = new UserActor();
            securable.AddActor(actor);
            return actor;
        }
    }
}