using Demo.Library.Security.Actors;
using Demo.Library.Security.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security
{
    public static class RuleExtensions
    {
        public static UserActor HavePermission(this UserActor actor, String permission)
        {
            var rule = new PermissionRule(actor, permission);
            actor.AddRule(rule);
            return actor;
        }
    }
}