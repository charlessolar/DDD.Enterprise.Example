using Demo.Library.Security.Actors;
using Demo.Library.Security.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Rules
{
    [TestFixture]
    public class PermissionRuleTests
    {
        [Test]
        public void Simple_permission()
        {
            var user = new Moq.Mock<UserActor>();
            user.Setup(x => x.Permissions).Returns(new List<String> { "test" });

            var rule = new PermissionRule(user.Object, "test");

            Assert.True(rule.IsAuthorized(null));
        }
        [Test]
        public void Simple_permission_fail()
        {
            var user = new Moq.Mock<UserActor>();
            user.Setup(x => x.Permissions).Returns(new List<String> { "test" });

            var rule = new PermissionRule(user.Object, "fail");

            Assert.False(rule.IsAuthorized(null));
        }
        [Test]
        public void Empty_permission()
        {
            var user = new Moq.Mock<UserActor>();
            user.Setup(x => x.Permissions).Returns(new List<String> { "test" });

            var rule = new PermissionRule(user.Object, "");

            Assert.True(rule.IsAuthorized(null));
        }
    }
}