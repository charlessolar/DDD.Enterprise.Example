using Demo.Library.Security.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Security.Rules
{
    public class PermissionRule : IRule
    {
        private readonly UserActor _user;
        public String Permission { get; private set; }

        public PermissionRule(UserActor user, String permission)
        {
            _user = user;
            Permission = permission;
        }

        public bool IsAuthorized(object instance)
        {
            return string.IsNullOrWhiteSpace(Permission) || _user.Permissions.Contains(Permission);
        }

        public const string DescriptionFormat = @"RequiredPermission_{{{0}}}";
        public string Description
        {
            get { return string.Format(DescriptionFormat, Permission); }
        }
    }
}