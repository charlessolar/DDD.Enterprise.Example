using Demo.Library.Security;
using Demo.Library.Security.Actors;
using Demo.Library.Security.Rules;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Library.Unit.Security.Extensions
{
    [TestFixture]
    public class RuleExtensionsTests
    {
        public Moq.Mock<UserActor> _actor;

        [SetUp]
        public void Setup()
        {
            _actor = new Moq.Mock<UserActor>();
        }

        [Test]
        public void Have_permission_adds_rule()
        {
            _actor.Setup(x => x.AddRule(Moq.It.IsAny<PermissionRule>())).Verifiable();
            _actor.Object.HavePermission("test");
            _actor.Verify(x => x.AddRule(Moq.It.IsAny<PermissionRule>()), Moq.Times.Once);
        }
    }
}